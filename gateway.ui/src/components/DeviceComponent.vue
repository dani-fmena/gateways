<template>
  <q-list v-if="devices.length" padding bordered separator class="rounded-borders" style="max-width: 800px; width: 600px;">
    <div class="row">
      <div class="col-4"><q-item-label header>Peripheral devices</q-item-label></div>
      <div class="col-5">
        <q-btn @click.stop="dialog = true" color="primary" class="gt-xs" size="12px" flat dense round icon="add_task" label="Add device">
          <q-tooltip class="bg-accent">add a device</q-tooltip>
        </q-btn>
      </div>
    </div>
      <q-item v-for="device in devices" :key="device.id">
        <q-item-section avatar top>
          <q-icon name="devices_other" color="black" size="34px" />
        </q-item-section>

        <q-item-section top class="col-2 gt-sm">
          <q-item-label class="q-mt-sm ellipsis">{{ device.vendor }}</q-item-label>
        </q-item-section>

        <q-item-section top>
          <q-item-label lines="1">
            <span class="text-weight-medium">UID: </span>
            <span class="text-grey-8"> {{ device.uid }}</span>
          </q-item-label>
          <q-item-label caption lines="1">
            status:
            {{ (device.isOnline ? 'online' : 'offline') }}
          </q-item-label>
          <q-item-label caption lines="1">
            Gateway ID:
            {{ device.gatewayId }}
          </q-item-label>
        </q-item-section>

        <q-item-section top side>
          <div class="text-grey-8 q-gutter-xs">
            <q-btn @click.stop="deleteDevice(device.id, device.uid)" class="gt-xs" size="md" flat dense round icon="delete">
              <q-tooltip class="bg-accent">remove a device</q-tooltip>
            </q-btn>
            <q-btn class="gt-xs hidden" size="12px" flat dense round icon="edit"><q-tooltip class="bg-accent">edit a device</q-tooltip></q-btn>
          </div>
        </q-item-section>
      </q-item>
  </q-list>
  <div v-else class="no-items absolute-center">
    <q-icon name="check" size="100px" color="primary"/>
    <div class="text-h5 text-primary text-center">
      No devices
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
          <div class="text-h6">Add a peripheral device</div>
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
                  v-model="newDevice.uid"
                  label="UID *"
                  mask="####-####-####-####"
                  fill-mask="#"
                  hint="A unique identifier numeric, Mask: ####-####-####-####"
                  :rules="[ val => val && val.length > 0  || 'Please write the identifier']"
                />

                <q-input
                  filled
                  v-model="newDevice.vendor"
                  label="Vendor *"
                  lazy-rules
                  :rules="[
                    val => val !== null && val !== '' || 'Please type a vendor'
                  ]"
                />
                  <q-input
                    filled
                    v-model="newDevice.created"
                    hint="Date created"
                  >
                    <template v-slot:prepend>
                      <q-icon name="event" class="cursor-pointer">
                        <q-popup-proxy cover transition-show="scale" transition-hide="scale">
                          <q-date v-model="newDevice.created" mask="YYYY-MM-DD HH:mm">
                            <div class="row items-center justify-end">
                              <q-btn v-close-popup label="Close" color="primary" flat />
                            </div>
                          </q-date>
                        </q-popup-proxy>
                      </q-icon>
                    </template>

                    <template v-slot:append>
                      <q-icon name="access_time" class="cursor-pointer">
                        <q-popup-proxy cover transition-show="scale" transition-hide="scale">
                          <q-time v-model="newDevice.created" mask="YYYY-MM-DD HH:mm" format24h>
                            <div class="row items-center justify-end">
                              <q-btn v-close-popup label="Close" color="primary" flat />
                            </div>
                          </q-time>
                        </q-popup-proxy>
                      </q-icon>
                    </template>
                  </q-input>

                <q-toggle v-model="newDevice.isOnline" label="Is online?" />

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
import { Device } from './models';
import { useQuasar, date } from 'quasar';
import { useDeviceStore } from 'src/stores/device-store';
import { useRouter } from 'vue-router';

function useDisplayTodo(devices: Ref<Device[]>) {
  const todoCount = computed(() => devices.value);
  return { todoCount };
}

export default defineComponent({
  name: 'DeviceComponent',
  props: {
    devices: {
      type: Array as PropType<Device[]>,
      default: () => []
    },
  },
  setup (props) {
    const $q = useQuasar()
    const deviceStore = useDeviceStore();
    const router = useRouter();

    const dialog = ref(false);
    const maximizedToggle = ref(true);
    const timeStamp = Date.now();

    const newDevice: Ref<Device> = ref({
          created: date.formatDate(timeStamp, 'YYYY-MM-DD HH:mm:ss'),
          gatewayId: Number(router.currentRoute.value.params.id.toString()),
          id: 0,
          isOnline: true,
          uid: '',
          vendor: '',
        })

    function deleteDevice (id: number, uid: string) {
      $q.dialog({
            title: 'Confirm',
            message: 'Are you sure you want to delete the device: '+uid+'?',
            cancel: true,
            persistent: true
          }).onOk(() => {
              deviceStore.deleteDevices(id)
              .then(() => {
                $q.notify('Device removed')
              })
              .catch(() => {
                $q.notify({type: 'negative', message: 'There was an error remove the device', icon: 'report_problem', position: 'top-right' })
              });
          })
    }

    return {
      newDevice, store:deviceStore,
      onSubmit () {
        deviceStore.addDevice(newDevice.value)
        .then(() => {
          $q.notify('Device added')
          dialog.value = false;
        })
        .catch(() => {
          $q.notify({type: 'negative', message: 'There was an error adding a device', icon: 'report_problem', position: 'top-right' })
          dialog.value = false;
        });
      },
      deleteDevice, ...useDisplayTodo(toRef(props, 'devices')),
      dialog, maximizedToggle
    };
  },
});
</script>

<style lang="scss">
  .no-items {
    opacity: 0.5;
  }
</style>
