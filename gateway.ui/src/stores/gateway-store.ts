import { Gateway, generateFakeData } from './../components/models';
import { defineStore } from 'pinia';
import { api } from 'boot/axios'

export type RootState = {
  gateways: Gateway[];
};

export const useGatewayStore = defineStore({
  id: 'gatewayStore',
  state: () =>
  ({
    gateways: [],
  } as RootState),

  getters: {
    getGateways(state) {
      return state.gateways
    }
  },

  actions: {
    async fetchGateways() {
      try {
        const data = await api.get('/v1/mngmt/AcGateway/rows')
        this.gateways = data.data
      }
      catch (error) {
        alert(error)
        console.log(error)
      }
    },
    createNewGateway(gateway: Gateway) {
      if (!gateway) return;

      this.gateways.push(gateway);
    },

    updateGateway(id: number, payload: Gateway) {
      if (!id || !payload) return;

      const index = this.findIndexById(id);

      if (index !== -1) {
        this.gateways[index] = generateFakeData();
      }
    },

    deleteGateway(index: number) {
      if (index === -1) return;
      this.gateways.splice(index, 1);
    },

    findIndexById(id: number) {
      return this.gateways.findIndex((gateway) => gateway.id === id);
    },
  },
});
