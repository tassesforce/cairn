<template>
    <div>
        <h1>Intérimaire</h1>
        <div id="agencyAuth">
            <auth ref="authMarkdown" :baseModel="baseModel.auth" :readOnly="readOnly"></auth>
        </div>
        <div id="updateCandidate" v-if="readOnly === false">
            <div>
                <br/>
                <span class="mandatory">*</span><input type="radio" id="m" value="M." v-model="baseModel.model.civility">
                <label for="m">M.</label>
                <input type="radio" id="mme" value="Mme" v-model="baseModel.model.civility">
                <label for="mme">Mme</label>
                <p v-if="errorLabels.civilityError.length > 0" class="error">{{errorLabels.civilityError}}</p><br v-else/>

                <span class="mandatory">*</span><input type="text" name="firstName" v-model="baseModel.model.firstName" placeholder="Prénom" v-on:blur="validateFirstName()" />
                <p v-if="errorLabels.firstNameError.length > 0" class="error">{{errorLabels.firstNameError}}</p><br v-else/>
                <span class="mandatory">*</span><input type="text" name="lastName" v-model="baseModel.model.lastName" placeholder="Nom de famille" v-on:blur="validateLastName()" />
                <p v-if="errorLabels.lastNameError.length > 0" class="error">{{errorLabels.lastNameError}}</p><br v-else/>
            </div>
            <div id="candidateContact">
                <div><contact ref="contactMarkdown" :baseModel="baseModel.model.contact" :readOnly="readOnly"></contact></div>
            </div>
            <div id="candidateAdress">
                <div><adress ref="adressMarkdown" :baseModel="baseModel.model.adress" :readOnly="readOnly"></adress></div>
            </div><br/>
            <span class="mandatory">*</span><input type="text" name="dispoDate" v-model="baseModel.model.dispoDate" placeholder="Date de disponibilité" v-on:blur="validateDispoDate()" />
            <p v-if="errorLabels.dispoDateError.length > 0" class="error">{{errorLabels.dispoDateError}}</p><br v-else/>
            <div>
                <h2>Handicap</h2>
                <input v-b-toggle.disability type="checkbox" name="disableStatus" v-model="baseModel.model.disability.disableStatus" placeholder="Statut handicapé" />
                <b-collapse id="disability" accordion="disability" class="mt-2">
                    <textarea name="disableDetails" v-model="baseModel.model.disability.disabilityDetails" placeholder="Détails" />
                </b-collapse>
            </div>
            <div>
                <h2>Mobilité</h2>
                <input type="checkbox" v-b-toggle.mobility name="accepted" v-model="baseModel.model.mobility.accepted" placeholder="Mobile" />
                <b-collapse id="mobility" accordion="mobility" class="mt-2">
                    <input type="text" name="transport" v-model="baseModel.model.mobility.transport" placeholder="Voiture, scooter..." />
                    <input type="text" name="transportArea" v-model="baseModel.model.mobility.transportArea" placeholder="Nombre de kilomètres max." />
                </b-collapse>
            </div>
            <div>Compétences à venir</div>
            <div>Modules à venir</div>
            <div>Expériences à venir</div>
            <div>Préférences à venir</div>
            <div>Partenaires à venir</div>
        </div>

        <div id="readCandidate" v-if="readOnly === true">
            <div>
                <p>{{baseModel.model.civility}} {{baseModel.model.firstName}} {{baseModel.model.lastName}}</p>
                <p>Disponible à partir du {{baseModel.model.dispoDate}}</p>
            </div>
            <div id="candidateContact">
                <div><contact :contact="baseModel.model.contact" :baseModel="baseModel.model.contact" :readOnly="readOnly"></contact></div>
            </div>
            <div id="candidateAdress">
                <div><adress :adress="baseModel.model.adress" :baseModel="baseModel.model.adress" :readOnly="readOnly"></adress></div>
            </div>
            <div>
                <h2>Handicap</h2>
                <!-- v-if -->
                <!-- <input type="checkbox" name="disableStatus" v-model="baseModel.model.disability.disableStatus" placeholder="Statut handicapé" /> -->
                <!-- <textarea name="disableDetails" v-model="baseModel.model.disability.disabilityDetails" placeholder="Détails" /> -->
            </div>
            <div>
                <h2>Mobilité</h2>
                <!-- v-if -->
                <!-- <input type="checkbox" name="accepted" v-model="baseModel.model.mobility.accepted" placeholder="Mobile" />
                <input type="text" name="transport" v-model="baseModel.model.mobility.transport" placeholder="Voiture, scooter..." />
                <input type="text" name="transportArea" v-model="baseModel.model.mobility.transportArea" placeholder="Nombre de kilomètres max." /> -->
            </div>
            <div>Compétences à venir</div>
            <div>Modules à venir</div>
            <div>Expériences à venir</div>
            <div>Préférences à venir</div>
            <div>Partenaires à venir</div>
        </div>
    </div>
</template>

<script>
    import adress from '../Adress.vue'
    import contact from '../Contact.vue'
    import auth from '../Auth.vue'

    // Regexp for only letters, '-' and space
    var regExp = /^[a-zA-Z- ]+$/;
    // regexp : date in the format DD/MM/YYYY
    var regExpDate = /^\d{2}[\/]\d{2}[\/]\d{4}$/;

    export default {
        name: 'CandidateAccount',
        data() {
            return {
                errorLabels: {
                    firstNameError: "",
                    lastNameError: "",
                    dispoDateError: "",
                    civilityError: ""
                },
                errorMessages: {
                    lastName: {
                        empty: "Merci de saisir votre nom de famille.",
                        format: "Le nom de famille est invalide."
                    },
                    firstName: {
                        empty: "Merci de saisir votre prénom.",
                        format: "Le prénom est invalide."
                    },
                    dateDispo: {
                        empty: "Aucune disponibilité ?",
                        format: "La date doit respecter le format suivant : 01/01/2020."
                    },
                    civility: {
                        empty: "Merci de choisir entre ces deux possibilité.",
                    }
                }
            };
        },
        components: {
            adress: adress,
            contact: contact,
            auth: auth
        },
        methods: {
            validateForm() {
                var valid = true;
                if (!this.validateCivility()) {
                    valid = false;
                }
                if (!this.validateLastName()) {
                    valid = false;
                }
                if (!this.validateFirstName()) {
                    valid = false;
                }
                if (!this.validateDispoDate()) {
                    valid = false;
                }
                if (!this.$refs.adressMarkdown.validateForm()) {
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
                if (typeof this.baseModel.model.firstName == "undefined" || this.baseModel.model.firstName == "") {
                    this.errorLabels.firstNameError = this.errorMessages.firstName.empty;
                    return false;
                } else if (!regExp.test(this.baseModel.model.firstName)) {
                    // only letters, - or space
                    this.errorLabels.firstNameError = this.errorMessages.firstName.format;
                    return false;
                } else {
                    this.errorLabels.firstNameError = "";
                }
                return true;
            },
            validateLastName() {
                if (typeof this.baseModel.model.lastName == "undefined" || this.baseModel.model.lastName == "") {
                    this.errorLabels.lastNameError = this.errorMessages.lastName.empty;
                    return false;
                } else if (!regExp.test(this.baseModel.model.lastName)) {
                    // only letters, - or space
                    this.errorLabels.lastNameError = this.errorMessages.lastName.format;
                    return false;
                } else {
                    this.errorLabels.lastNameError = "";
                }
                return true;
            },
            validateDispoDate() {
                if (typeof this.baseModel.model.dispoDate == "undefined" || this.baseModel.model.dispoDate == "") {
                    this.errorLabels.dispoDateError = this.errorMessages.dateDispo.empty;
                    return false;
                } else if (!regExpDate.test(this.baseModel.model.dispoDate)) {
                    this.errorLabels.dispoDateError = this.errorMessages.dateDispo.format;
                    return false;
                } else {
                    this.errorLabels.dispoDateError = "";
                }
                return true;
            },
            validateCivility() {
                if (typeof this.baseModel.model.civility == "undefined" || this.baseModel.model.civility == "") {
                    this.errorLabels.civilityError = this.errorMessages.civility.empty;
                    return false;
                } else {
                    this.errorLabels.civilityError = "";
                }
                return true;
            }
        },
        props: ['baseModel', 'readOnly']
    }
</script>

<style>

</style>