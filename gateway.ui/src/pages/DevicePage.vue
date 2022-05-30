<template>
  <q-page class="row items-center justify-evenly">
    Devices
    {{$route.params.id}}

    <device-component :devices="devices"/>

    <!-- <div v-if="devices.length">
     {{devices}}
    </div>
    <div v-else class="no-items absolute-center">
      <q-icon name="check" size="100px" color="primary"/>
      <div class="text-h5 text-primary text-center">
        No devices
    </div>
  </div>-->

  </q-page>
</template>

<script lang="ts">
import DeviceComponent from 'components/DeviceComponent.vue';
import { defineComponent, onMounted, computed } from 'vue';
import { useDeviceStore } from 'src/stores/device-store';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: 'DevicePage',
  components: { DeviceComponent },
  setup () {
    const deviceStore = useDeviceStore();
    const router = useRouter();

    const getDevices = computed(() => {
      return deviceStore.getDevices
    })

    onMounted(() => {
      deviceStore.fetchDevices(router.currentRoute.value.params.id.toString())
    });

    return { devices:getDevices };
  }
});
</script>

<style lang="scss">
  .no-items {
    opacity: 0.5;
  }
</style>
