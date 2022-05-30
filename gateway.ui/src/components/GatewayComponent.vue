<template>
  <q-list v-if="gateways.length" padding bordered separator class="rounded-borders" style="max-width: 800px; width: 600px;">
    <div class="row">
      <div class="col-4"><q-item-label header>Gateways</q-item-label></div>
      <div class="col-5">
        <q-btn @click.stop="dialog = true" color="primary" class="gt-xs" size="12px" flat dense round icon="add_task" label="Add gateway">
          <q-tooltip class="bg-accent">add a gateway</q-tooltip>
        </q-btn>
      </div>
    </div>
      <q-item  v-for="gateway in gateways" :key="gateway.id">
        <q-item-section avatar top>
        <router-link  :to="`/gateway/${gateway.id}`">
          <q-icon name="router" color="black" size="34px" />
          </router-link>
        </q-item-section>


        <q-item-section clickable top class="col-2 gt-sm">
        <router-link  :to="`/gateway/${gateway.id}`">
          <q-item-label class="q-mt-sm ellipsis">{{ gateway.name }}</q-item-label>
          </router-link>
        </q-item-section>


        <q-item-section top :to="`/gateway/${gateway.id}`">
          <q-item-label lines="1">
            <span class="text-weight-medium">Serial number: </span>
            <span class="text-grey-8"> {{ gateway.serialNumber }}</span>
          </q-item-label>
          <q-item-label caption lines="1">
            Address:
            {{ gateway.ipAddress }}
          </q-item-label>
          <q-item-label caption lines="1">
            Associated devices:
            {{ gateway.peripheralsAssociated }}
          </q-item-label>
        </q-item-section>

        <q-item-section top side>
          <div class="text-grey-8 q-gutter-xs">
            <q-btn @click.stop="deleteGateway(gateway.id, gateway.serialNumber)" class="gt-xs" size="md" flat dense round icon="delete">
              <q-tooltip class="bg-accent">remove a gateway</q-tooltip>
            </q-btn>
            <q-btn class="gt-xs hidden" flat dense round icon="edit"><q-tooltip class="bg-accent">edit a gateway</q-tooltip></q-btn>
          </div>
        </q-item-section>
      </q-item>
  </q-list>
  <div v-else class="no-items absolute-center">
      <q-icon name="check" size="100px" color="primary"/>
      <div class="text-h5 text-primary text-center">
        No gateways
      </div>
  </div>

      <q-dialog
      v-model="dialog"
      persistent
      :maximized="maximizedToggle"
      transition-show="slide-up"
      transition-hide="slide-down"
    >
      <q-card class="text-black">
        <q-bar>
          <q-space />

          <q-btn dense flat icon="minimize" @click="maximizedToggle = false" :disable="!maximizedToggle">
            <q-tooltip v-if="maximizedToggle" class="bg-white text-primary">Minimize</q-tooltip>
          </q-btn>
          <q-btn dense flat icon="crop_square" @click="maximizedToggle = true" :disable="maximizedToggle">
            <q-tooltip v-if="!maximizedToggle" class="bg-white text-primary">Maximize</q-tooltip>
          </q-btn>
          <q-btn dense flat icon="close" v-close-popup>
            <q-tooltip class="bg-white text-primary">Close</q-tooltip>
          </q-btn>
        </q-bar>

        <q-card-section>
          <div class="text-h6">Add a gateway</div>
        </q-card-section>

        <q-card-section class="q-pt-none row items-center justify-evenly">
          <div class="q-pa-md" style="max-width: 600px; width: 500px">

              <q-form
                ref="qform"
                @submit="onSubmit"
                class="q-gutter-md"
              >

                <q-input
                  filled
                  v-model="newGateway.serialNumber"
                  label="Serial number *"
                  mask="XXXX-XXXX-XXXX-XXXX"
                  fill-mask="#"
                  hint="A unique serial number (string), Mask: XXXX-XXXX-XXXX-XXXX"
                  :rules="[ val => val && val.length > 0  || 'Please write the identifier']"
                />

                <q-input
                  filled
                  v-model="newGateway.name"
                  label="Vendor *"
                  lazy-rules
                  :rules="[
                    val => val !== null && val !== '' || 'Please type a vendor'
                  ]"
                />

                <q-input
                  v-model="newGateway.ipAddress"
                  label="IPv4 address *"
                  :rules="[
                    $rules.required('IPV4 address required'),
                    $rules.ipAddress(),
                  ]"
                />

                <div>
                  <q-btn label="Submit" type="submit" color="primary"/>
                </div>
              </q-form>

          </div>
        </q-card-section>
      </q-card>
    </q-dialog>
</template>

<script lang="ts">
import {
  defineComponent,
  PropType,
  computed,
  toRef,
  Ref,
  ref,
} from 'vue';
import { Gateway } from './models';
import { useQuasar } from 'quasar';
import { useGatewayStore } from 'src/stores/gateway-store';

function useDisplayGateways(gateways: Ref<Gateway[]>) {
  const gws = computed(() => gateways.value);
  return { gws };
}

export default defineComponent({
  name: 'GatewayComponent',
  props: {
    gateways: {
      type: Array as PropType<Gateway[]>,
      default: () => []
    },
  },
  setup (props) {
    const $q = useQuasar()
    const gatewayStore = useGatewayStore();
    const dialog = ref(false);
    const maximizedToggle = ref(true);
    const newGateway: Ref<Gateway> = ref({
      id: 0,
      name: '',
      serialNumber: '',
      ipAddress: '127.0.0.1',
      peripheralsAssociated: 0,
    })

    function deleteGateway (id: number, serialNumber: string) {
      $q.dialog({
            title: 'Confirm',
            message: 'Are you sure you want to delete the gateway: '+serialNumber+'?',
            cancel: true,
            persistent: true
          }).onOk(() => {
              gatewayStore.deleteGateway(id)
              .then(() => {
                $q.notify('Gateway removed')
              })
              .catch(() => {
                $q.notify({type: 'negative', message: 'There was an error remove the gateway', icon: 'report_problem', position: 'top-right' })
              });
          })
    }
    return {
      newGateway, maximizedToggle, dialog,
      onSubmit () {
        gatewayStore.addGateway(newGateway.value)
        .then(() => {
          $q.notify('Gateway added')
          dialog.value = false;
        })
        .catch(() => {
          $q.notify({type: 'negative', message: 'There was an error adding a gateway', icon: 'report_problem', position: 'top-right' })
          dialog.value = false;
        });
      },
      deleteGateway,
      ...useDisplayGateways(toRef(props, 'gateways')) };
  },
});
</script>

<style lang="scss">
  .no-items {
    opacity: 0.5;
  }
</style>
