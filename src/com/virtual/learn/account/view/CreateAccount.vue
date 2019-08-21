<template>
    <div id="page">
        <h1>Mon compte</h1>
        <div id="content">
            <div class="form">
                <!-- Choose the account type -->
                <div v-if = "isUpdate == false">
                    <!-- <b-button v-b-toggle.agency class="m-1" @click="changeAccountType('agency')">Agence</b-button> -->
                    <!-- <b-button v-b-toggle.candidate class="m-1" @click="changeAccountType('candidate')">Intérimaire</b-button> -->
                    <!-- <b-button v-b-toggle.company class="m-1" @click="changeAccountType('company')">Entreprise</b-button> -->
                </div>
                <!-- <b-collapse id="agency" accordion="account-type" class="mt-2"> -->
                    <agencyAccount ref="agency" :baseModel="this.account" :readOnly="false"></agencyAccount>
                <!-- </b-collapse> -->
                <!-- <b-collapse id="company" accordion="account-type" class="mt-2">
                    <companyAccount ref="company" :baseModel="this.account" :readOnly="false"></companyAccount>
                </b-collapse>
                <b-collapse id="candidate" accordion="account-type" class="mt-2">
                    <candidateAccount ref="candidate" :baseModel="this.account" :readOnly="false"></candidateAccount>
                </b-collapse> -->
            </div>
        </div>
        <div>
            <button type="button" v-on:click="cancel()">Annuler</button>
            <button type="button" v-on:click="create()">Créer</button>
            <!-- <router-link v-on:click.native="create()" replace tag="button" to="/readAccount" >Créer</router-link> -->
            <!-- <router-link to="/readAccount"> -->
                <!-- <span v-on:click="create()">Créer</span> -->
            <!-- </router-link> -->
        </div>
    </div>
</template>

<script>
    import agencyAccount from './agency/AgencyAccount.vue'
    import candidateAccount from './candidate/CandidateAccount.vue'
    import companyAccount from './company/CompanyAccount.vue'
    import axios from 'axios'
    export default {
        name: 'CreateAccount',
        data() {
            return {
                isUpdate: false,
                account: {
                    auth: {
                        login: "",
                        password: ""
                    },
                    model: {
                        userId: "",
                        name: "",
                        siret: "",
                        sector: "",
                        adress: {
                            number: "",
                            label: "",
                            complement1: "",
                            complement2: "",
                            postalCode: "",
                            town: "",
                            country: ""
                        },
                        contact: {
                            mail: "",
                            phone: "",
                            web: ""
                        },
                        responsible: {
                            firstName: "",
                            lastName: "",
                            position: ""
                        },
                        disability: {},
                        fund: {
                            adress: {}
                        },
                        mobility: {}
                    }
                },
                accountType: "agency"
            };
        },
        methods: {
            cancel() {
                this.$router.replace({name: "login"});
            },
            create() {
                console.log("create");
                var valid = this.validateForm();
                console.log("valid : " + valid);
                if (valid) {
                    var url = "";
                    if ("agency" == this.accountType) {
                        url = "/Account/CreateAgencyAccount";
                    } else if ("company" == this.accountType) {
                        url = "/Account/CreateCompanyAccount";
                    } else if ("candidate" == this.accountType) {
                        url = "/Account/CreateCandidateAccount";
                    }
                    this.account.accountType = this.accountType;
                    this.account.phone = this.account.model.contact.phone;
                    this.account.mail = this.account.model.contact.mail;

                    // mandatory
                    // the instance that 'this' reference in axios doesn't point to the vue instance
                    var instance = this;
                    axios({
                        method: 'post',
                        url: url,
                        data: this.account
                    }).then(function(response) {
                        instance.$router.push("/readAccount");
                    }).catch(function (error) {
                        console.log(error);
                    });
                }
            },
            validateForm() {
                if ("agency" == this.accountType) {
                    return this.$refs.agency.validateForm();
                } else if ("company" == this.accountType) {
                    return this.$refs.company.validateForm();
                } else if ("candidate" == this.accountType) {
                    return this.$refs.candidate.validateForm();
                }

            },
            changeAccountType(newAccountType) {
                this.accountType = newAccountType;
                this.account = {
                    auth: {
                        login: "",
                        password: ""
                    },
                    model: {
                        userId: "",
                        name: "",
                        siret: "",
                        sector: "",
                        adress: {
                            number: "",
                            label: "",
                            complement1: "",
                            complement2: "",
                            postalCode: "",
                            town: "",
                            country: ""
                        },
                        contact: {
                            mail: "",
                            phone: "",
                            web: ""
                        },
                        responsible: {
                            firstName: "",
                            lastName: "",
                            position: ""
                        },
                        disability: {},
                        fund: {
                            adress: {}
                        },
                        mobility: {}
                    }
                }
            }
        },
        components: {
            'agencyAccount': agencyAccount,
            'candidateAccount': candidateAccount,
            'companyAccount': companyAccount
        }
    }
</script>

<style scoped>
    #content, .form {
        background-color: #FFFFFF;
        border: 1px solid #CCCCCC;
        padding: 20px;
        margin-top: 10px;
    }
</style>