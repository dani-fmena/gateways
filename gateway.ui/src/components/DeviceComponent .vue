<template>
  <q-list padding bordered separator class="rounded-borders" style="max-width: 800px; width: 600px;">
      <q-item-label header>Devices</q-item-label>
      <q-item clickable v-for="(device, index) in devices" :key="device.id">
        <q-item-section avatar top>
          <q-icon name="router" color="black" size="34px" />
        </q-item-section>

        <q-item-section top class="col-2 gt-sm">
          <q-item-label class="q-mt-sm ellipsis">{{ device.vendor }}</q-item-label>
        </q-item-section>

        <q-item-section top>
          <q-item-label lines="1">
            <span class="text-weight-medium">Serial number: </span>
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
            <q-btn @click.stop="deleteDevice(index, device.id)" class="gt-xs" size="12px" flat dense round icon="delete">
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

    function deleteDevice (index: number, serialNumber: number) {
      $q.dialog({
            title: 'Confirm',
            message: 'Are you sure you want to delete the device: '+serialNumber+'?',
            cancel: true,
            persistent: true
          }).onOk(() => {
            console.log('aquii ', index)
            deviceStore.deleteDevices(index);
            $q.notify('Device removed')
          })
    }
    return { deleteDevice, ...useDisplayTodo(toRef(props, 'devices')) };
  },
});
</script>
