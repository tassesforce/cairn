<template>
    <div style="height: 100%">
        <div id="left-menu" class="menu-left">
            <div class="menu-accueil" v-on:click="GoHome()"><img src="../../../../../../../wwwroot/assets/ic_person.png"/>Accueil</div>
            <div class="menu-catalogue" v-on:click="GoToCatalog()"><img src="../../../../../../../wwwroot/assets/open-book-top-view.png"/>Catalogue</div>
            <div class="menu-immersion"><img src="../../../../../../../wwwroot/assets/ic_candidate.png"/>Immersions</div>
        </div>
        <div id="top-menu" class="top-menu">
            <div class="logo"></div>
            <div class="menu-title">
                <label>Learn in Virtual environment</label>
            </div>
            <div class="info">
                <div class="log-off" v-on:click="logoff()"></div>
                <div class="menu-text">
                    <label>{{this.Name}}</label>
                </div>
            </div>
        </div>
        <div id="content" class="content">
            <router-view></router-view>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'
    
    export default {
        name: 'Profile',
        data() {
            return {
                Name: ""
            }
        }, 
        created() {
            // mandatory
            // the instance that 'this' reference in axios doesn't point to the vue instance
            var instance = this;
            axios({
                method: 'get',
                url: "/Account/GetAccount",
            }).then(function(response) {
                instance.Name = response.data.account.name;
            }).catch(function (error) {
                console.log(error);
            });
            // this.$router.push({ name: 'readCollaboratorAccount', params: { readOnly: true, search: true} });
            this.$router.push("/readAccount");
        },
        methods: {
            logoff() {
                var instance = this;
                axios({
                    method: 'get',
                    url: 'auth/LogOff'
                }).then(function(response) {
                    instance.$router.push({name: "login"});
                }).catch(function (err) {
                    console.log(err);
                });
            },
            GoToCatalog() {
                this.$router.push({name: "catalog"});
            },
            GoHome() {
                this.$router.push("/readAccount");
            }
        }
    }
</script>

<style scoped>
    @import '../../../../../../../wwwroot/css/form/form.css';
    @import '../../../../../../../wwwroot/css/form/button.css';
    @import '../../../../../../../wwwroot/css/profile/menu.css';
</style>