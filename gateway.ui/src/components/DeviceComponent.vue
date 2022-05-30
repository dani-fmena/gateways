<template>
  <q-list padding bordered separator class="rounded-borders" style="max-width: 800px; width: 600px;">
      <q-item-label header>Peripheral devices</q-item-label>
      <q-item clickable v-for="device in devices" :key="device.id">
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
            {{ device.isOnline }}
          </q-item-label>
          <q-item-label caption lines="1">
            Gateway:
            {{ device.gatewayId }}
          </q-item-label>
        </q-item-section>

        <q-item-section top side>
          <div class="text-grey-8 q-gutter-xs">
            <q-btn @click.stop="deleteDevice(device.id, device.uid)" class="gt-xs" size="12px" flat dense round icon="delete">
              <q-tooltip class="bg-accent">remove a device</q-tooltip>
            </q-btn>
            <q-btn class="gt-xs" size="12px" flat dense round icon="edit"><q-tooltip class="bg-accent">edit a device</q-tooltip></q-btn>
          </div>
        </q-item-section>
      </q-item>
  </q-list>
</template>

<script lang="ts">
import {
  defineComponent,
  PropType,
  computed,
  toRef,
  Ref,
} from 'vue';
import { Device } from './models';
import { useQuasar } from 'quasar';
import { useDeviceStore } from 'src/stores/device-store';

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
    return { deleteDevice, ...useDisplayTodo(toRef(props, 'devices')) };
  },
});
</script>
