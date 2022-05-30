<template>
  <q-layout view="lHh LpR fff">
    <q-header bordered>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />

        <q-toolbar-title>
        <div class="1-px-lg q-pt-xl q-mb-md">
          <div class="text-h3">Gateways app</div>
          <div class="text-subtitle1">{{ todaysDate }}</div>
        </div>
        </q-toolbar-title>
      </q-toolbar>
    </q-header>
      <q-drawer
        v-model="leftDrawerOpen"
        show-if-above
        :width="250"
        :breakpoint="600"
      >
        <q-scroll-area style="height: calc(100% - 150px); margin-top: 150px; border-right: 1px solid #ddd">
          <q-list padding>
            <q-item to="/" exact clickable v-ripple>
              <q-item-section avatar>
                <q-icon name="router" />
              </q-item-section>

              <q-item-section>
                Gateways
              </q-item-section>
            </q-item>

            <q-item  to="/about" exact clickable v-ripple>
              <q-item-section avatar>
                <q-icon name="info" />
              </q-item-section>

              <q-item-section>
                About
              </q-item-section>
            </q-item>

          </q-list>
        </q-scroll-area>

        <q-img class="absolute-top" style="height: 144px; background-color: var(--q-primary);">
          <div class="absolute-bottom bg-transparent">
            <q-avatar size="56px" class="q-mb-sm">
              <img src="src/assets/boy-avatar.png">
            </q-avatar>
            <div class="text-weight-bold">Daniel Mena</div>
            <div>@mena</div>
          </div>
        </q-img>
      </q-drawer>


    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { date } from 'quasar'

export default defineComponent({
  name: 'MainLayout',

  components: {},

  setup () {
    const leftDrawerOpen = ref(false)

    return {
      leftDrawerOpen,
      toggleLeftDrawer () {
        leftDrawerOpen.value = !leftDrawerOpen.value
      }
    }
  },
  computed: {
    todaysDate() {
      const timeStamp = Date.now()
      return date.formatDate(timeStamp, 'dddd D MMMM')
    }
  },
});
</script>
