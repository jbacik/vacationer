<template>
  <b-container class="dashboard-container">
      <b-row>
          <b-col class="dashboard-container__metrics">
              <div class="stats-group">
                  <label for="taken">Taken</label>
                  <div class="stats-group__value" id="taken">
                      <span>{{this.takenHours}}</span>
                      <b-button variant="default" size="sm" @click="showTakenList">
                          <b-icon-list></b-icon-list>
                      </b-button>
                  </div>
              </div>
              <div class="stats-group">
                  <label for="planned">Planned</label>
                  <div class="stats-group__value" id="planned">
                      <span>{{this.plannedHours}}</span>
                      <b-button variant="default" size="sm" @click="showPlannedList">
                          <b-icon-list></b-icon-list>
                      </b-button>
                </div>
              </div>
                <div class="stats-group">
                  <label for="unused">Available</label>
                    <div class="stats-group__value" id="unused">{{availableHours}}</div>
              </div>
              <div class="stats-group">
                  <label for="unused">Total Unused</label>
                    <div class="stats-group__value" id="unused">{{unusedHours}}</div>
              </div>
          </b-col>
          <b-col class="dashboard-container__summary">
              <HomeChart :chartdata="chartData" :options="chartOptions"/>
              <div class="chart-info">
                  <div>Effective On: {{userInfo.vacationPoolStartDate}}</div>
                  <div>Total Hours: {{userInfo.vacationPoolHours}}</div>
              </div>
          </b-col>
      </b-row>
  </b-container>
</template>

<script>
import HomeChart from './HomeChart.vue';
import {data} from '../shared';

export default {
    name: "HomeDashboard",
    components: {HomeChart},
    props: {
        hours: {
            type: Array,
            default: () => []
        }
    },
    data() {
        return {
            userInfo: {},
            chartData: null,
            chartOptions: null
        }
    },    
    async created() {
        await this.loadUserInfo();
    },
    methods : {
        async loadUserInfo() {
            this.userInfo = await data.getUserInfo();
        },
        showTakenList() {
            this.$emit('toggleList', 'taken');
        },
        showPlannedList() {
            this.$emit('toggleList', 'planned');
        }
    },
    computed: {
        takenHours() {
            const vactionHours = this.hours.filter(vc => !vc.isPlanned)
                                    .map(vc => vc.vacationHours);

            return vactionHours.reduce((a, b) => a + b, 0);
        },
        plannedHours() {
            const vactionHours = this.hours.filter(vc => vc.isPlanned)
                                    .map(vc => vc.vacationHours);

            return vactionHours.reduce((a, b) => a + b, 0);
        },
        unusedHours() {
            return this.userInfo.vacationPoolHours - this.takenHours;
        },
        availableHours() {
            return this.userInfo.vacationPoolHours - (this.takenHours + this.plannedHours);
        }
    }
}
</script>

<style scoped>
    .dashboard-container {
        margin: 2rem 2%;
    }

    .stats-group label {
        font-size: 1.8rem;
        font-weight:700;
        text-decoration:underline;
    }

    .stats-group .stats-group__value span {
        font-size: 1.6rem;
    }

    .stats-group .stats-group__value button {
        font-size: 1rem;
    }

    .chart-info {
        font-size:0.8rem;
        color:#999;
        text-align: left;
        padding-left:5%;
    }
</style>