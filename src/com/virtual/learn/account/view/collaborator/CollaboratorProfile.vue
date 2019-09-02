<template>
    <div style="height: 100%">
        <div id="left-menu" class="menu-left">
            <div class="menu-accueil"><img src="../../../../../../../wwwroot/assets/ic_person.png"/>Accueil</div>
            <div class="menu-catalogue"  v-on:click="GoToCatalog()"><img src="../../../../../../../wwwroot/assets/open-book-top-view.png"/>Catalogue</div>
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
                    <label>{{firstName}} {{lastName}}</label>
                </div>
            </div>
            <!-- <button class="logoff" type="button" v-on:click="logoff()">Déconnexion</button> -->
        </div>
        <div id="content" class="content">
            <router-view v-on:account-loaded="displayPersonnalName"></router-view>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'
    
    export default {
        name: 'Profile',
        data() {
            return {
                firstName: "",
                lastName: ""
            }
        }, 
        created() {
            // mandatory
            // the instance that 'this' reference in axios doesn't point to the vue instance
            var instance = this;
            axios({
                method: 'get',
                url: "/Account/GetCollaboratorAccount"
            }).then(function(response) {
                instance.firstName = response.data.account.firstName;
                instance.lastName = response.data.account.lastName;
            }).catch(function (error) {
                console.log(error);
            });
            this.$router.push({ name: 'readCollaboratorAccount', params: { readOnly: true, search: true} });
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
                console.log("Go modules");
                this.$router.push({name: "catalogCollaborator"});
            // },
            // displayPersonnalName(account) {
            //     console.log("checkpoint 2");
            //     Object.assign(this.firstName, account.firstName);
            }
        }
    }
</script>

<style>
    @import '../../../../../../../wwwroot/css/profile/menu.css';
</style>