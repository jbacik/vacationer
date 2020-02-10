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
                <tr v-for="item in items" :key="item.id">
                    <td>
                        <span v-show="selectedItem != item">{{getDisplayDate(item)}}</span>
                        <b-input-group v-show="selectedItem == item">
                            <b-input v-model="item.vacationStartDate" placeholder="Start Date" aria-label="Start Date" />
                            <b-input v-model="item.vacationEndDate" placeholder="End Date" aria-label="End Date" />
                        </b-input-group>                        
                    </td>
                    <td>
                        <span v-show="selectedItem != item">{{item.vacationHours}}</span>
                        <b-input v-show="selectedItem == item" class="text-center" v-model="item.vacationHours" placeholder="Hours" aria-label="Hours" />
                    </td>
                    <td>
                        <b-button-group>
                            <b-button variant="secondary" size="sm" @click="editDate(item)">
                                <b-icon-pencil>Edit</b-icon-pencil>
                            </b-button>
                            <b-button class="text-danger" variant="outline-default" size="sm" @click="removeItem(item.id)">
                                <b-icon-x-square>Remove</b-icon-x-square>
                            </b-button>
                        </b-button-group>
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
        },
        isPlanned: {
            type: String,
            default: () => "true"
        }
    },
    data() {
        return {
            newItem: {},
            selectedItem: {}
        }
    },
    methods: {
        addNewItem () {
            this.$emit('addHours', this.newItem, (this.isPlanned === "true"));
            this.newItem = {};
        },
        removeItem (id) {
            this.$emit('removeItem', id);
        },
        getDisplayDate (item) {
            if (item.vacationStartDate === item.vacationEndDate)
                return item.vacationStartDate; //format(item.vacationStartDate, 'MM/DD');
            else
                return `${item.vacationStartDate} - ${item.vacationEndDate}` //`${format(item.vacationStartDate, 'MM/DD')} - ${format(item.vacationEndDate, 'MM/DD')}`;                
        },
        editDate (item) {
            this.selectedItem = item;
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