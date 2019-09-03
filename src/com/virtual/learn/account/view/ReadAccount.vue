<template>
    <div id="page">
        <h1>Mon compte</h1>
        <div id="content">
            <div class="form">
                <div id="agency" v-if="accountType === 'agency'">
                    <agencyAccount ref="agency" :baseModel="this.account" :readOnly="true"></agencyAccount>
                </div>
                <div id="company" v-if="accountType === 'company'">
                    <companyAccount ref="company" :baseModel="this.account" :readOnly="true"></companyAccount>
                </div>
                <div id="candidate" v-if="accountType === 'candidate'">
                    <candidateAccount ref="candidate" :baseModel="this.account" :readOnly="true"></candidateAccount>
                </div>
            </div>
        </div>
        <div>
            <button type="button" v-on:click="update()">Modifier</button>
        </div>
    </div>
</template>

<script>
    import agencyAccount from './agency/AgencyAccount.vue'
    import candidateAccount from './candidate/CandidateAccount.vue'
    import companyAccount from './company/CompanyAccount.vue'
    import axios from 'axios'
    
    export default {
        name: 'ReadAccount',
        data() {
            return {
                accountType: "",
                account: {
                    model: {}
                }
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
                instance.accountType = response.data.accountType;
                instance.account.model = response.data.account;
                instance.account.auth = response.data.auth;
            }).catch(function (error) {
                console.log(error);
            });
        },
        methods: {
            update() {
                var route = 'update' + this.accountType.charAt(0).toUpperCase() + this.accountType.slice(1) + "Account";
                this.$router.push({ name: route, params: { id: this.account.model.userId } });
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