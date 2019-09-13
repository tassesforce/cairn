<template>
    <div style="height: 100%">
        <div id="content" class="content">
            <div class="box-shadow content-login">
                <input type="text" name="login" v-model="input.login" placeholder="Username" autofocus />
                <p v-if="loginError.length > 0" class="error">{{loginError}}</p>
                <input type="password" name="password" v-model="input.password" placeholder="Password" />
                <p v-if="passwordError.length > 0" class="error">{{passwordError}}</p>
                <div id="buttons" class="bloc-buttons-login">
                    <a class="button highlight-button" v-on:click="login()" @shortkey="login()">Me connecter</a>
                </div>
                <div class="actions-supplementaires">
                    <div class="separator"></div>
                    Nouvel utilisateur ? <a v-on:click="create()">Je crée mon compte</a>
                </div>
            </div>
        </div>
    </div>
</template>
<!--
<template>
    <div id="login">
        <h1>Login</h1>
        <div>
            <input type="text" name="login" v-model="input.login" placeholder="Username" autofocus />
            <p v-if="loginError.length > 0" class="error">{{loginError}}</p><br v-else/>
            <input type="password" name="password" v-model="input.password" placeholder="Password" />
            <p v-if="passwordError.length > 0" class="error">{{passwordError}}</p>
        </div>
        <button type="button" v-on:click="login()" v-shortkey="['enter']" @shortkey="login()">Login</button>
        <button type="button" v-on:click="create()">Créer mon compte</button>
        <!-- <a v-on:click="lostIds()">J'ai oublié mes identifiants</a>
    </div>
</template>
 -->
<script>
    import axios from 'axios'
    export default {
        name: 'Login',
        data() {
            return {
                loginError: "",
                passwordError: "",
                errorMessages: {
                    login: {
                        empty: "Merci de saisir un identifiant. En cas d'oubli, n'hésitez pas à utilisez l'action 'Identifiant oublié'"
                    },
                    password: {
                        empty: "Merci de saisir un mot de passe. En cas d'oubli, n'hésitez pas à utilisez l'action 'Mot de passe oublié'"
                    }
                },
                input: {
                    login: "",
                    password: ""
                }
            }
        },
        methods: {
            login() {
                var instance = this;
                axios({
                    method: 'post',
                    url: 'auth/AuthenticateUser',
                    data: instance.input
                }).then(function(response) {
                    instance.$router.push({name: response.data + "Profile"});
                }).catch(function (err) {
                    console.log(err);
                });

            },
            lostIds() {
                var instance = this;
                axios({
                    method: 'post',
                    url: 'auth/LostIdsUser',
                    data: instance.input
                }).then(function(response) {
                    instance.$router.push("/readAccount");
                }).catch(function (err) {
                    console.log(err);
                });

            },
            create() {
                this.$router.push({name: "createAccount"});
            }
        }
    }
</script>

<style scoped>
    @import '../../../../../../wwwroot/css/profile/menu.css';
    @import '../../../../../../wwwroot/css/form/form.css';
    @import '../../../../../../wwwroot/css/form/button.css';
    @import '../../../../../../wwwroot/css/auth/login.css';

input {
    width: 100%;
}
</style>