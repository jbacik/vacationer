<template>
  <div id="app">
    <b-navbar type="dark" variant="info">
      <b-navbar-brand><a href="#" @click.prevent="showDashboard">vacationer</a></b-navbar-brand>
    </b-navbar>
    <HomeDashboard v-show="activePage == 'dashboard'" :hours="hours" @toggleList="toggleList"/>
    
    <HourListing title="Taken Hours" 
      v-show="activePage == 'taken'" 
      :items="takenHours"
      isPlanned="false" 
      @addHours="addHours"
      @removeItem="removeItem" />
      
    <HourListing title="Planned Hours" 
      v-show="activePage == 'planned'" 
      :items="plannedHours" 
      isPlanned="true" 
      @addHours="addHours"
      @removeItem="removeItem" />
  </div>
</template>

<script>
import HomeDashboard from './components/HomeDashboard.vue';
import HourListing from './components/HourListing.vue';
import {data} from './shared';

export default {
  name: 'app',
  components: {
    HomeDashboard,
    HourListing
  },
  data () {
    return {
      activePage: 'dashboard',
      hours: [],
      newIdx: 0
    }
  },
  methods: {
    showDashboard() {
      this.activePage = 'dashboard';
    },
    toggleList(target) {
      this.activePage = target;
    },
    addHours(newItem, isPlanned) {
      console.log('add taken', newItem);

      this.hours.push({
          id: this.newIdx++,
          userId: 3,
          vacationStartDate: newItem.startDate,
          vacationEndDate: newItem.endDate,
          vacationHours: parseFloat(newItem.hours),
          isPlanned: isPlanned
      });
    },
    removeItem(id) {
      
      var indexToRemove = this.hours.findIndex((el) => el.id === id);
      console.log('remove hours', id, indexToRemove);
      if (indexToRemove !== undefined && indexToRemove >= 0){
        this.hours.splice(indexToRemove, 1);
      }
    },
    async loadUserHours() {
      this.hours = await data.getHours(3);
      this.newIdx = this.hours.length;
    }
  },
  async created() {
    await this.loadUserHours();
  },
  computed: {
    takenHours() {
      return this.hours.filter(vc => !vc.isPlanned);
    },
    plannedHours() {
      return this.hours.filter(vc => vc.isPlanned);
    }
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
.navbar a {
  color: #fff;
  text-decoration: none;
}

.navbar a:hover {
  color: #fff;
  font-weight:700;
  text-decoration: none;
}

</style>
