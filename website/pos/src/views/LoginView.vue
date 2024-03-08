<template>
    <div id="loginBox">
        <div id="loginDetails">
            <form>
                <div class="input-container ib1">
                    <input v-model="email" id="email" class="input" placeholder="" type="text" required>
                    <label for="email" class="placeholder">Email</label>
                </div>
                <div class="input-container ib2">
                    <input v-model="password" id="password" class="input" placeholder="" type="password" required>
                    <label for="password" class="placeholder">Password</label>
                </div>
            </form>
        </div>
        <button @click="login">LOGIN</button>
    </div>
</template>

<script>
import axios from 'axios'

export default {
    data(){
        return {
            email: "",
            password: ""
        }
    },
    methods: {
        
        async login() {
            axios.post("http://localhost:8080/api/Users/login", 
                {
                    email: this.email,
                    password: this.password
                }, {
                headers: {
                    'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjAiLCJuYmYiOjE3MDk3MjUzMzYsImV4cCI6MTcwOTcyODkzNiwiaWF0IjoxNzA5NzI1MzM2fQ.cWkStvWKBCqkaczVuxOr6_FmT2PlI4lokSuJN_fhZwQ'
                }
            }).then(response => {
                //go to next page here
                console.log(response)
            }).catch(e => {
                console.log(e)
            })
        }
    }
}

</script>

<style>

:root {
    background: rgb(2,32,36);
    background: linear-gradient(40deg, rgba(20,34,36,1) 25%, rgba(34,34,34,1) 75%);
}

.input {
  background-color: #303245;
  border-radius: 12px;
  border: 0;
  box-sizing: border-box;
  color: #eee;
  font-size: 18px;
  height: 100%;
  outline: 0;
  padding: 4px 20px 0;
  width: 100%;
}

.input-container {
  height: 50px;
  position: relative;
  width: 100%;
  padding-top: 20px;
}

input[type='password'] {
    font-family: Verdana;
    letter-spacing: 0.125em;
}

.ib1 {
  margin-top: 40px;
}

.ib2 {
  margin-top: 30px;
}

.placeholder {
    color: #65657b;
    font-family: sans-serif;
    left: 20px;
    line-height: 14px;
    pointer-events: none;
    position: absolute;
    transform-origin: 0 50%;
    transition: transform 200ms, color 200ms;
    transition-delay: 50ms;
    top: 55%;
}

.input:focus ~ .placeholder,
.input:not(:placeholder-shown) ~ .placeholder {
    transform: translateY(-3vh) scale(0.85);
}

.input:not(:placeholder-shown) ~ .placeholder {
    color: #808097;
}

.input:focus ~ .placeholder {
    color: #2f77dc;
    background: rgb(48,50,69);
    background: linear-gradient(0deg, rgba(48,50,69,1) 50%, rgba(56,55,55,1) 50%);
    padding: 5px;
}

.input:focus {
    border: solid 2px #2f77dc;
}

#loginBox {
    max-width: 25%;
    max-height: 30%;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    padding: 200px;
    padding-bottom: 300px;
    border-radius: 25px;
    background-color: #383737;
}

button{
    margin-top: 50px;
    width: 100%;
    background-color: #ffffff;
    color: #080710;
    padding: 15px 0;
    font-size: 18px;
    font-weight: 600;
    border-radius: 5px;
    cursor: pointer;
}

</style>