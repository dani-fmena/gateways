<template>
  <q-list v-if="gateways.length" padding bordered separator class="rounded-borders" style="max-width: 800px; width: 600px;">
      <q-item-label header>Gateways</q-item-label>
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
            <q-btn @click.stop="deleteGateway(gateway.id, gateway.serialNumber)" class="gt-xs" size="12px" flat dense round icon="delete">
              <q-tooltip class="bg-accent">remove a gateway</q-tooltip>
            </q-btn>
            <q-btn class="gt-xs" size="12px" flat dense round icon="edit"><q-tooltip class="bg-accent">edit a gateway</q-tooltip></q-btn>
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
</template>

<script lang="ts">
import {
  defineComponent,
  PropType,
  computed,
  toRef,
  Ref,
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
    return { deleteGateway, ...useDisplayGateways(toRef(props, 'gateways')) };
  },
});
</script>

<style lang="scss">
  .no-items {
    opacity: 0.5;
  }
</style>
