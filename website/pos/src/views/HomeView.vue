<script setup>


</script>

<template>
  <div class="page">
    <h1 class="Title">welcome</h1>
      <div class="grid-container">
        <router-link to="/Sales" class="grid-item grid-item-large">
          <h2>sales</h2>
        </router-link>
        <router-link to="/Storage" class="grid-item">
          <h2>Storage</h2>
        </router-link>
        <router-link to="/Appointments" class="grid-item">
          <h2>Appointments</h2>
        </router-link>
        <router-link to="/Appointments" class="grid-item">
          <h2>Appointments</h2>
        </router-link>

      </div>
  </div>

</template>

<style scoped>
.page {

}
.grid-container {
  display: grid;
  grid-template-columns: auto auto auto   ;
  align-content: center;
  padding: 10px;
  gap: 20px;
  align-items: center;

}

.grid-item{
  background: red;
  text-align: center;
  padding: 20px;
  height: 20vh;
  border-radius: 10px;
  align-items: center;
  text-decoration: none;
}

.grid-item-large {
  grid-column: span 3;
}

.Title {
  margin-top: 100px;
  text-align: center;
}

h2 {
  color: white;
  font-size: 2em;
  font-weight: 500;
  text-align: center;
  text-decoration: none;
}

</style>