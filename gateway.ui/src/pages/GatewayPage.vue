<template>
  <q-page class="row items-center justify-evenly">
    <gateway-component :gateways="gateways"/>
  </q-page>
</template>

<script lang="ts">
import GatewayComponent from 'components/GatewayComponent.vue';
import { defineComponent, onMounted, computed } from 'vue';
import { useGatewayStore } from 'src/stores/gateway-store';
import { useQuasar } from 'quasar'

export default defineComponent({
  name: 'GatewayPage',
  components: { GatewayComponent },
  setup () {
    const $q = useQuasar()
    $q.loading.show({
      delay: 400 // ms
    })

    const gatewayStore = useGatewayStore();

    const getGateways = computed(() => {
      return gatewayStore.getGateways
    })

    onMounted(() => {
      gatewayStore.fetchGateways()
      $q.loading.hide()
    });

    return { gateways:getGateways };
  }
});
</script>
