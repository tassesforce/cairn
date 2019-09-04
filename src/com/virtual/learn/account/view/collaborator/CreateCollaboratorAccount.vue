<template>
    <div>
        <label class="title">Profil Collaborateur</label>
        <div class="flex-container">
            <div class="w100p">
                <label class="mid-bloc-title">Mon agence</label>
                <div class="w100p column box-shadow">
                    <p>{{this.referentName}}</p>
                    <adress :baseModel="this.referentAdress" :readOnly="true" />
                </div>
            </div>
        </div>

        <div class="titleLeftColumns titleColumns">
            <label class="mid-bloc-title">Informations du collaborateur</label>
        </div>
        <div class="titleRightColumns titleColumns">
            <label class="mid-bloc-title">Coordonnées du collaborateur</label>
        </div>
        <div id="updateCollaborator" class="flex-container">
            <div class="w90p column box-shadow">
                <input type="text" name="firstName" v-model="account.model.firstName" placeholder="FirstName" v-on:blur="validateFirstName()" />
                <p v-if="errorLabels.firstNameError.length > 0" class="error">{{errorLabels.firstNameError}}</p><br v-else/>
                <input type="text" name="lastName" v-model="account.model.lastName" placeholder="LastName" v-on:blur="validateLastName()" />
                <p v-if="errorLabels.lastNameError.length > 0" class="error">{{errorLabels.lastNameError}}</p><br v-else/>
                <input type="text" name="position" v-model="account.model.position" placeholder="Position" />
            </div>
            <div class="w90p column box-shadow fright " style="margin-left:10%;">
                <contact ref="contactMarkdown" :baseModel="account.model.contact" :readOnly="false"></contact>
            </div>
            
        </div>

       <div class="flex-container">
            <div class="w100p">
                <label class="mid-bloc-title">Identifiants de connexion</label>
                <div class="w100p column box-shadow">
                    <div id="agencyAuth">
                        <auth ref="authMarkdown" :baseModel="account.auth" :readOnly="false"></auth>
                    </div>
                </div>
            </div>
        </div>

        <div id="buttons" class="bloc-buttons">
            <a class="button downlight-button" v-on:click="cancel()">Annuler</a>
            <a class="button highlight-button" v-on:click="save()">Créer</a>
        </div>
    </div>
</template>

<script>
    import adress from '../Adress.vue'
    import contact from '../Contact.vue'
    import auth from '../Auth.vue'
    import axios from 'axios'

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
                referentAdress: {}
            };
        },
        components: {
            'adress': adress,
            'contact': contact,
            'auth': auth
        },
        created() {
            this.readOnly = this.$route.params.readOnly;
            if (this.$route.params.search == true) {
                // we are in the case of a read/update of a collaborator account, so we directly get all the informations (account + referent)

                // mandatory
                // the instance that 'this' reference in axios doesn't point to the vue instance
                var instance = this;
                axios({
                    method: 'get',
                    url: "/Account/GetAccount"
                }).then(function(response) {
                    instance.account.model = response.data.account;
                    instance.account.auth = response.data.auth;
                    instance.referentName = response.data.name;
                    instance.referentAdress = response.data.adress;
                }).catch(function (error) {
                    console.log(error);
                });
            } else {
                // we are in the case of the creation of a collaborator account, so we use the informations of the agency account currently used
                this.referentName = this.$route.params.name;
                this.referentAdress = this.$route.params.adress;
            }
        },
        methods: {
            save() {
                var valid = this.validateForm();
                if (valid) {
                    this.account.phone = this.account.model.contact.phone;
                    this.account.mail = this.account.model.contact.mail;

                    var url = "/Account/CreateCollaboratorAccount";
                    var method = 'post';
                    if (this.isUpdate == true) {
                        var url = "/Account/UpdateCollaboratorAccount";
                        var method = 'put';
                    }

                    // mandatory
                    // the instance that 'this' reference in axios doesn't point to the vue instance
                    var instance = this;
                    axios({
                        method: method,
                        url: url,
                        data: this.account
                    }).then(function(response) {
                        instance.$router.push("/readAccount");
                    }).catch(function (error) {
                        console.log(error);
                    });
                }
            },
            cancel() {
                this.$router.go(-1);
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

</style>