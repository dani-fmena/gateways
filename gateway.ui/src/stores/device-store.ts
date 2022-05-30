import { Device } from '../components/models';
import { defineStore } from 'pinia';
import { api } from 'boot/axios'

export type DeviceState = {
  devices: Device[];
};

export const useDeviceStore = defineStore({
  id: 'deviceStore',
  state: () =>
  ({
    devices: [],
  } as DeviceState),

  getters: {
    getDevices(state) {
      return state.devices
    }
  },

  actions: {
    async fetchDevices(gatewayId: string|string[]) {
      console.log("fetchDevices: ", gatewayId)
      try {
        const data = await api.get('/v1/mngmt/AcPeripheral/rows/gateway/'+gatewayId)
        this.devices = data.data
      }
      catch (error) {
        alert(error)
        console.log(error)
      }
    },

    deleteDevices(index: number) {
      // const index = this.findIndexById(id);
      console.log("gw: ", this.devices)
      if (index === -1) return;
      this.devices.splice(index, 1);
    },

    findIndexById(id: number) {
      return this.devices.findIndex((device) => device.id === id);
    },

    filterByGatewayId(gatewayId: number) {
      return this.devices.findIndex((device) => device.gatewayId === gatewayId);
    },
  },
});
