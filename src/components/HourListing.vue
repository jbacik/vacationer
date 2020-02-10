<template>
  <b-container class="listing-container">
      <ListHeader :title="title">/</ListHeader>
      <table class="table striped-hover">
          <thead>
              <tr>
                  <th>Dates</th>
                  <th>Hours</th>
                  <th></th>
                </tr>
          </thead>
          <tbody>
                <tr v-for="item in items" :key="item.id" >
                    <td>
                        <span>{{getDisplayDate(item)}}</span>
                    </td>
                    <td>
                        <span>{{item.vacationHours}}</span>
                    </td>
                    <td>
                        <b-button variant="secondary" size="sm" @click="editDate">
                            <b-icon-pencil>Edit</b-icon-pencil>
                        </b-button>
                    </td>
                </tr>
          </tbody>
          <tfoot>
              <tr>
                  <td>
                      <div>
                        <b-input-group>
                            <b-input v-model="newItem.startDate" placeholder="Start Date" aria-label="Start Date" />
                            <b-input v-model="newItem.endDate" placeholder="End Date" aria-label="End Date"/>
                        </b-input-group>
                      </div>
                  </td>
                  <td>
                      <b-input v-model="newItem.hours" placeholder="Hours" aria-label="Hours" />
                  </td>
                  <td>
                      <b-button variant="primary" size="sm" @click="addNewItem">
                          <b-icon-upload>Save</b-icon-upload>
                      </b-button>
                  </td>
              </tr>
          </tfoot>
      </table>
  </b-container>
</template>

<script>
import ListHeader from './ListHeader.vue';
import { format } from 'date-fns';

export default {
    name: "HoursListing",
    components: {
        ListHeader
    },
    props: {
        title: {
            type: String,
            default: () => ''
        },
        items: {
            type: Array,
            default: () => []
        }
    },
    data() {
        return {
            newItem: {}
        }
    },
    methods: {
        addNewItem () {
            //this.$emit('addTaken', this.newItem);
            // //emit
            // this.items.push({
            //     id: this.newIdx++,
            //     userId: 3,
            //     vacationStartDate: this.newItem.startDate,
            //     vacationEndDate: this.newItem.endDate,
            //     vacationHours: this.newItem.hours
            // });
            this.newItem = {};
        },
        getDisplayDate(item) {
            if (item.vacationStartDate === item.vacationEndDate)
                return item.vacationStartDate; //format(item.vacationStartDate, 'MM/DD');
            else
                return `${item.vacationStartDate} - ${item.vacationEndDate}` //`${format(item.vacationStartDate, 'MM/DD')} - ${format(item.vacationEndDate, 'MM/DD')}`;                
        },
        editDate() {
            console.log('edit it');
        }
    },
    filters: {
        shortDate: function(value) {
            return format(value, 'MM/DD');
        }
    }
}
</script>

<style>

</style>