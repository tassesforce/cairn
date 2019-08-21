<template>
    <div id="login">
        <div>
            <button class="logoff" type="button" v-on:click="logoff()">Déconnexion</button>
        </div>
        <router-view></router-view>
    </div>
</template>

<script>
    import axios from 'axios'
    
    export default {
        name: 'Profile',
        data() {
            return {
                
            }
        }, 
        created() {
            this.$router.push("/readAccount");
        },
        methods: {
            logoff() {
                var instance = this;
                axios({
                    method: 'get',
                    url: 'auth/LogOff',
                    data: instance.input
                }).then(function(response) {
                    instance.$router.push({name: "login"});
                }).catch(function (err) {
                    console.log(err);
                });
            }
        }
    }
</script>

<style scoped>
.logOff {
    float: right;
}

</style>