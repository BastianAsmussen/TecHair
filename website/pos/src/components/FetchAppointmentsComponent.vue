<script>
import axios from "axios";
const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjAiLCJuYmYiOjE3MDk3MjUzMzYsImV4cCI6MTcwOTcyODkzNiwiaWF0IjoxNzA5NzI1MzM2fQ.cWkStvWKBCqkaczVuxOr6_FmT2PlI4lokSuJN_fhZwQ";

export default {
    data() {
        return {
            axios: axios.create({
                baseURL: 'http://localhost:8080',
                headers: {'Authorization': `Bearer ${token}`}
            }),
            appointments: [],
        }
    },
    methods: {
        get() {
            this.axios.get('/api/Appointments').then(response => {
                this.appointments = response.data;
                console.log(this.appointments); // Add this line


                this.$emit('appointments-fetched', this.appointments);
            }).catch(error => {
                console.log(error);
            });
        }
    },
    mounted() {
        this.get();
    }
}
</script>


