<template>
  <div id="app" style="height: 100%">
    <router-link v-if="authenticated" to="/login" v-on:click.native="logout()" replace>Logout</router-link>
      <!-- the router outlet, where all matched components would ber viewed -->
    <router-view></router-view>
  </div>
</template>


<script>
    export default {
        name: 'App',
        data() {
            return {
                authenticated: false
            }
        },
        mounted() {
            if(!this.authenticated) {
                this.$router.replace({ name: "login" });
            }
        },
        methods: {
            setAuthenticated(status) {
                this.authenticated = status;
            },
            logout() {
                this.authenticated = false;
            }
        }
    }
</script>

<style>
@import '../../../../../../../wwwroot/css/profile/menu.css';
    body {
        background-color: #F0F0F0;
        height: 100%;
    }
    h1 {
        padding: 0;
        margin-top: 0;
    }
    #app {
        width: 1024px;
        margin: auto;
    }
</style>