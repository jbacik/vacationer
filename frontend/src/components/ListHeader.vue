<template>
    <div class="content-title-group">
      <h2 class="title">{{title}}</h2>
        <b-button variant="outline-primary" @click="handleAdd" aria-label="add">
            <b-icon-plus aria-hidden="true"></b-icon-plus>
        </b-button>
  </div>
</template>

<script>
export default {
    name: "ListHeader",
    props: {
        title: {
            type: String,
            default: () => ''
        }
    },
    methods: {
        handleAdd(){
            console.log('Add a new thing.');
            //this.$emit('add');
        }
    }
}
</script>

<style>
.content-title-group{
    display:flex;
    justify-content:space-between;
}
</style>