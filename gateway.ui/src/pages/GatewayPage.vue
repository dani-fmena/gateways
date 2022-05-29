<template>
  <q-page class="row items-center justify-evenly">
    <gateway-component :gateways="gateways"/>
  </q-page>
</template>

<script lang="ts">
import GatewayComponent from 'components/GatewayComponent.vue';
import { defineComponent, onMounted, computed } from 'vue';
import { useGatewayStore } from 'src/stores/gateway-store';

export default defineComponent({
  name: 'GatewayPage',
  components: { GatewayComponent },
  setup () {
    const gatewayStore = useGatewayStore();

    const getGateways = computed(() => {
      return gatewayStore.getGateways
    })

    onMounted(() => {
      gatewayStore.fetchUsers()
    });

    return { gateways:getGateways };
  }
});
</script>
