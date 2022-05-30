<template>
  <q-list v-if="gateways.length" padding bordered separator class="rounded-borders" style="max-width: 800px; width: 600px;">
      <q-item-label header>Gateways</q-item-label>
      <q-item clickable v-for="(gateway, index) in gateways" :key="gateway.id" :to="`/gateway/${gateway.id}`">
        <q-item-section avatar top>
          <q-icon name="router" color="black" size="34px" />
        </q-item-section>

        <q-item-section top class="col-2 gt-sm">
          <q-item-label class="q-mt-sm ellipsis">{{ gateway.name }}</q-item-label>
        </q-item-section>

        <q-item-section top>
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
            <q-btn @click.stop="deleteGateway(index, gateway.serialNumber)" class="gt-xs" size="12px" flat dense round icon="delete">
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

    function deleteGateway (index: number, serialNumber: string) {
      $q.dialog({
            title: 'Confirm',
            message: 'Are you sure you want to delete the gateway: '+serialNumber+'?',
            cancel: true,
            persistent: true
          }).onOk(() => {
            console.log('aquii ', index)
            gatewayStore.deleteGateway(index);
            $q.notify('Gateway removed')
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
