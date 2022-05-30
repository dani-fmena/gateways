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
    },
  },

  actions: {
    async fetchDevices(gatewayId: string) {
      try {
        const data = await api.get('/v1/mngmt/AcPeripheral/rows/gateway/'+gatewayId)
        this.devices = data.data
      }
      catch (error) {
        alert(error)
        console.log(error)
      }
    },
    async addDevice(device: Device) {
      console.log('store -> addDevice: ', device)

      return api.post('/v1/mngmt/AcPeripheral', device)
      .then( () => {
          this.devices.push(device);
       })
       .catch((error) => {
          // alert(error)
          // console.log(error);
          throw error
       });
    },
    async deleteDevices(id: number) {
      const config = {
        data: [id]
      }
      return api.delete('/v1/mngmt/AcPeripheral/batch', config)
      .then( () => {
          const index = this.findIndexById(id);
          this.devices.splice(index, 1);
       })
       .catch((error) => {
          // alert(error)
          // console.log(error);
          throw error
       });
    },

    findIndexById(id: number) {
      return this.devices.findIndex((device) => device.id === id);
    },

    filterByGatewayId(gatewayId: number) {
      return this.devices.findIndex((device) => device.gatewayId === gatewayId);
    },
  },
});
