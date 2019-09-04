<template>
    <div>
        <label class="title">Mon profil Agence</label>
        <div class="titleLeftColumns titleColumns">
            <label class="mid-bloc-title">Coordonnées de l'agence</label>
        </div>
        <div class="titleRightColumns titleColumns">
            <label class="mid-bloc-title">Coordonnées du contact principal</label>
        </div>
        <div id="updateAgency" v-if="readOnly === false" class="flex-container">
            <div class="w90p column box-shadow">
                <input type="text" name="name" v-model="baseModel.model.name" placeholder="Name" v-on:blur="validateName()" />
                <p v-if="errorLabels.nameError.length > 0" class="error">{{errorLabels.nameError}}</p><br v-else/>
                <input type="text" name="siret" v-model="baseModel.model.siret" placeholder="Siret" v-on:blur="validateSiret()" />
                <p v-if="errorLabels.siretError.length > 0" class="error">{{errorLabels.siretError}}</p><br v-else/>
                <adress ref="adressMarkdown" :baseModel="baseModel.model.adress" :readOnly="readOnly"></adress>
            </div>
            <div class="w90p column box-shadow fright " style="margin-left:10%;">
                <responsible ref="responsibleMarkdown" :baseModel="baseModel.model.responsible" :readOnly="readOnly"></responsible>
                <contact ref="contactMarkdown" :baseModel="baseModel.model.contact" :readOnly="readOnly"></contact>
            </div>
        </div>

        <div id="readAgency" v-if="readOnly === true" class="flex-container"> 
            <div class="w90p column box-shadow">
                <p>{{baseModel.model.name}}</p>
                <p>{{baseModel.model.siret}}</p>
                <adress ref="adressMarkdown" :baseModel="baseModel.model.adress" :readOnly="readOnly"></adress>
            </div>
            <div class="w90p column box-shadow fright " style="margin-left:10%;">
                <responsible ref="responsibleMarkdown" :baseModel="baseModel.model.responsible" :readOnly="readOnly"></responsible>
                <contact ref="contactMarkdown" :baseModel="baseModel.model.contact" :readOnly="readOnly"></contact>
            </div>
            
        </div>

        <div class="flex-container">
            <div class="w100p">
                <label class="mid-bloc-title">Identifiants de connexion</label>
                <div class="w100p column box-shadow">
                    <div id="agencyAuth">
                        <auth ref="authMarkdown" :baseModel="baseModel.auth" :readOnly="readOnly"></auth>
                    </div>
                </div>
            </div>
        </div>

        <br/>
        
        <div id="collaborators" class="collaborators-container">
            <div class="w100p">
                <label class="collaborators-title">Collaborateurs</label>
                <a class="button highlight-button-create title-button" v-on:click="createCollaborator()">Créer</a>
            </div>
            <div class="w100p grid-container">
                <div class="collaborators box-shadow" v-for="collab in baseModel.model.collaborators" v-bind:key="collab.id" >
                    <div><img src="../../../images/icons/trash.png" align= "right" class="icons" v-on:click="deleteCollab(collab.id)"/></div>
                    <div v-on:click="seeCollab(collab.id)">
                        {{collab.firstName}} {{collab.lastName}}<br/>
                        <div v-if="typeof collab.position != 'undefined' && collab.position.length > 0">
                            {{collab.position}}
                        </div>
                        <contact :baseModel="collab.contact" :readOnly="true"></contact>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import adress from '../Adress.vue'
    import contact from '../Contact.vue'
    import responsible from '../Responsible.vue'
    import auth from '../Auth.vue'
    import axios from 'axios'

    export default {
        name: 'AgencyAccount',
        data() {
            return {
                siretSize: 14,
                errorLabels: {
                    siretError: "",
                    nameError: ""
                },
                errorMessages: {
                    siret: {
                        empty: "Merci de saisir un numéro SIRET.",
                        size: "Le SIRET doit être composé de 14 caractères.",
                        format: "Le SIRET n'est composé que de numéros."
                    },
                    name: {
                        empty: "Le nom de société est obligatoire."
                    }
                }
            };
        },
        components: {
            'adress': adress,
            'contact': contact,
            'responsible': responsible,
            'auth': auth
        },
        methods: {
            validateForm() {
                // Validate the form, to see if we can perform the final action
                var valid = true;
                if (!this.validateSiret()) {
                    valid = false;
                }
                if (!this.validateName()) {
                    valid = false;
                }
                if (!this.$refs.adressMarkdown.validateForm()) {
                    valid = false;
                }
                if (!this.$refs.contactMarkdown.validateForm()) {
                    valid = false;
                }
                if (!this.$refs.responsibleMarkdown.validateForm()) {
                    valid = false;
                }
                if (!this.$refs.authMarkdown.validateForm()) {
                    valid = false;
                }
                return valid;
            },
            validateSiret() {
                // Validate the SIRET number => only numbers, size of 14 and mandatory
                // Only numbers regexp
                var regExpNumber = /^[0-9]+$/;

                if (typeof this.baseModel.model.siret == "undefined" || this.baseModel.model.siret.length == 0) {
                    this.errorLabels.siretError = this.errorMessages.siret.empty;
                    return false;
                } else if (this.baseModel.model.siret.length != this.siretSize) {
                    this.errorLabels.siretError = this.errorMessages.siret.size;
                    return false;
                } else if (!regExpNumber.test(this.baseModel.model.siret)) {
                    this.errorLabels.siretError = this.errorMessages.siret.format;
                    return false;
                } else {
                    this.errorLabels.siretError = "";
                }
                return true;
            },
            validateName() {
                // Validate the presence of a name for this agency
                if (typeof this.baseModel.model.name == "undefined" || this.baseModel.model.name.length == 0) {
                    this.errorLabels.nameError = this.errorMessages.name.empty;
                    return false;
                } else {
                    this.errorLabels.nameError = "";
                }
                return true;
            },
            seeCollab(idCollab) {
                this.$router.push({ name: 'readEmbeddedCollaboratorAccount', params: { readOnly: true, search: true, id: idCollab, embedded: true} });
            },
            deleteCollab(idCollab) {
                this.$dialog.confirm('La suppression de ce compte est définitive. Souhaitez-vous continuer ?')
                .then(function () {
                    if (typeof idCollab != 'undefined' && idCollab.length > 0) {
                        // mandatory
                        // the instance that 'this' reference in axios doesn't point to the vue instance
                        var instance = this;
                        var url = "/Account/DeleteCollaboratorAccount?userId=" + idCollab;
                        axios({
                            method: 'delete',
                            url: url
                        }).then(function(response) {
                            //TODO supprimer le collab de la liste
                        }).catch(function (error) {
                            console.log(error);
                        });
                    } else {
                        // show error is vide (should not happen)
                    }
                })
                .catch(function () {
                    // show error TODO
                });
                
            },
            createCollaborator() {
                this.$router.push({ name: 'createEmbeddedCollaboratorAccount', params: { readOnly: false, name: this.baseModel.model.name, adress: this.baseModel.model.adress, search: false } });
            }
        },
        props: ['baseModel', 'readOnly']
        
    }
</script>

<style scoped>
    @import '../../../../../../../wwwroot/css/account/collaborator.css';
</style>