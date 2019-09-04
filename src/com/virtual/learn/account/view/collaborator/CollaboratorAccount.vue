<template>
    <div style="width: 100%">
        <label class="bloc-title">Mon agence</label>
        <div class="bloc box-shadow">
            <p>{{this.referentName}}</p>
            <adress :baseModel="this.referentAdress" :readOnly="true" />
        </div>

        <div class="flex-container">
            <div class="mid-bloc">
                <label class="mid-bloc-title">Informations personnelles</label>
                <div class="w90p column box-shadow">
                    <div id="updateCollaborator" v-if="this.readOnly === false">
                        <input type="text" name="firstName" v-model="account.model.firstName" placeholder="FirstName" v-on:blur="validateFirstName()" />
                        <p v-if="errorLabels.firstNameError.length > 0" class="error">{{errorLabels.firstNameError}}</p><br v-else/>
                        <input type="text" name="lastName" v-model="account.model.lastName" placeholder="LastName" v-on:blur="validateLastName()" />
                        <p v-if="errorLabels.lastNameError.length > 0" class="error">{{errorLabels.lastNameError}}</p><br v-else/>
                        <input type="text" name="position" v-model="account.model.position" placeholder="Position" />
                    </div>

                    <div id="readCollaborator" v-if="this.readOnly === true">
                        <label>{{account.model.firstName}} {{account.model.lastName}}</label>
                        <label>{{account.model.position}}</label>
                    </div>
                </div>
            </div>

            <div class="mid-bloc">
                <div class="right-bloc">
                    <label class="mid-bloc-title">Coordonnées du compte</label>
                    <div class="w90p column box-shadow">
                        <contact ref="contactMarkdown" :baseModel="account.model.contact" :readOnly="this.readOnly"></contact>
                    </div>
                </div>
            </div>
        </div>

        <label class="bloc-title">Identifiants de connexion</label>
        <div id="collaboratorAuth" class="bloc box-shadow" v-if="embedded == false">
            <auth ref="authMarkdown" :baseModel="account.auth" :readOnly="this.readOnly"></auth>
        </div><br/>


        <br/>
        <div id="buttons" class="bloc-buttons">
            <a class="button highlight-button" v-on:click="update()" v-if="this.readOnly == true && this.embedded == false">Modifier</a>
            <a class="button downlight-button" v-on:click="cancel()" v-if="this.readOnly == false || this.embedded == true">Annuler</a>
            <a class="button highlight-button" v-on:click="save()" v-if="this.readOnly == false">Enregistrer</a>

            <!-- <button type="button" v-on:click="update()" v-if="this.readOnly == true && this.embedded == false">Modifier</button>
            <button type="button" v-on:click="cancel()" v-if="this.readOnly == false || this.embedded == true">Annuler</button>
            <button type="button" v-on:click="save()" v-if="this.readOnly == false">Enregistrer</button> -->
        </div>
    </div>
</template>

<script>
    import adress from '../Adress.vue';
    import contact from '../Contact.vue';
    import auth from '../Auth.vue';
    import axios from 'axios';

    // Regexp for only letters, '-' and space
    var regExp = /^[a-zA-Z- ]+$/;

    export default {
        name: 'CollaboratorAccount',
        data() {
            return {
                errorLabels: {
                    lastNameError: "",
                    firstNameError: ""
                },
                errorMessages: {
                    lastName: {
                        empty: "Merci de saisir un nom de famille.",
                        format: "Le nom de famille est invalide."
                    },
                    firstName: {
                        empty: "Merci de saisir un prénom.",
                        format: "Le prénom est invalide."
                    }
                },
                account: {
                    auth: {
                        login: "",
                        password: ""
                    },
                    model: {
                        firstName: "",
                        lastName: "",
                        position: "",
                        contact: {

                        }
                    }
                },
                referentName: "",
                referentAdress: {},
                readOnly: false,
                embedded: false
            };
        },
        components: {
            'adress': adress,
            'contact': contact,
            'auth': auth
        },
        created() {
            this.readOnly = this.$route.params.readOnly;
            var id = this.$route.params.id;
            console.log("id : " + id);
            if (typeof this.$route.params.embedded != 'undefined' && this.$route.params.embedded == true) {
                this.embedded = true;
            }
            if (this.$route.params.search == true) {
                // we are in the case of a read/update of a collaborator account, so we directly get all the informations (account + referent)

                // mandatory
                // the instance that 'this' reference in axios doesn't point to the vue instance
                var instance = this;
                var url = "/Account/GetCollaboratorAccount";
                if (typeof id != 'undefined' && id.length > 0) {
                    url = "/Account/GetCollaboratorAccount?userId=" + id;
                }
                axios({
                    method: 'get',
                    url: url
                }).then(function(response) {
                    instance.account.model = response.data.account;
                    instance.account.auth = response.data.auth;
                    instance.referentName = response.data.name;
                    instance.referentAdress = response.data.adress;
                    instance.cachedAccount = Object.assign({}, instance.account);
                }).catch(function (error) {
                    console.log(error);
                });
            } else {
                // we are in the case of the creation of a collaborator account, so we use the informations of the agency account currently used
                this.referentName = this.$route.params.name;
                this.referentAdress = this.$route.params.adress;
            }
        },
        mounted() {
            this.cachedAccount = Object.assign({}, this.account);
        },
        methods: {
            save() {
                var valid = this.validateForm();
                if (valid) {
                    this.account.phone = this.account.model.contact.phone;
                    this.account.mail = this.account.model.contact.mail;
                    // mandatory
                    // the instance that 'this' reference in axios doesn't point to the vue instance
                    var instance = this;
                    axios({
                        method: 'put',
                        url: "/Account/UpdateCollaboratorAccount",
                        data: this.account
                    }).then(function(response) {
                        instance.readOnly = true;
                    }).catch(function (error) {
                        console.log(error);
                    });
                }
            },
            cancel() {
                if (this.embedded == true) {
                    this.$router.push({name: 'readAgencyAccount'});
                } else {
                    this.readOnly = true;
                    console.log(JSON.stringify(this.cachedAccount));
                    this.account = Object.assign({}, this.cachedAccount);
                }
            },
            update() {
                this.readOnly = false;
            },
            validateForm() {
                // Validate the form, to see if we can perform the final action
                var valid = true;
                if (!this.validateFirstName()) {
                    valid = false;
                }
                if (!this.validateLastName()) {
                    valid = false;
                }
                if (!this.$refs.contactMarkdown.validateForm()) {
                    valid = false;
                }
                if (!this.$refs.authMarkdown.validateForm()) {
                    valid = false;
                }
                return valid;
            },
            validateFirstName() {
                if (typeof this.account.model.firstName == "undefined" || this.account.model.firstName == "") {
                    this.errorLabels.firstNameError = this.errorMessages.firstName.empty;
                    return false;
                } else if (!regExp.test(this.account.model.firstName)) {
                    // only letters, - or space
                    this.errorLabels.firstNameError = this.errorMessages.firstName.format;
                    return false;
                } else {
                    this.errorLabels.firstNameError = "";
                }
                return true;
            },
            validateLastName() {
                if (typeof this.account.model.lastName == "undefined" || this.account.model.lastName == "") {
                    this.errorLabels.lastNameError = this.errorMessages.lastName.empty;
                    return false;
                } else if (!regExp.test(this.account.model.lastName)) {
                    // only letters, - or space
                    this.errorLabels.lastNameError = this.errorMessages.lastName.format;
                    return false;
                } else {
                    this.errorLabels.lastNameError = "";
                }
                return true;
            }
        }
        
    }
</script>

<style>
    @import '../../../../../../../wwwroot/css/form/form.css';
</style>