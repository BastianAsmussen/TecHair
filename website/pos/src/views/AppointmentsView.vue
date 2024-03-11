<script >
import FetchAppointment from '@/components/FetchAppointmentsComponent.vue';

export default {
    name: 'AppointmentsView',
    components: {
        FetchAppointment
    },
    data() {
        return {
            appointments: [],
        }
    },
    methods: {
        handleAppointmentsFetched(appointments) {
            this.appointments = appointments;
        },
    },
}
</script>

<template>
    <div>

        <FetchAppointment @appointments-fetched="handleAppointmentsFetched" />

        <h1>Schedule</h1>
        <button @click="$router.push('/')">Home</button>

        <table>
            <thead>
            <tr>
                <th>Date</th>
                <th>Status</th>
                <th>Barber Email</th>
                <th>Barber Name</th>
                <th>Customer Email</th>
                <th>Customer Name</th>
                <th>Price</th>
                <th>Notes</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="appointment in appointments" :key="appointment.appointmentId">
                <td>{{ appointment.date }}</td>
                <td>{{ appointment.status }}</td>
                <td>{{ appointment.barber && appointment.barber.user ? appointment.barber.user.email : '' }}</td>
                <td>{{ appointment.barber && appointment.barber.user ? appointment.barber.user.name : '' }}</td>
                <td>{{ appointment.customer ? appointment.customer.email : '' }}</td>
                <td>{{ appointment.customer ? appointment.customer.name : '' }}</td>
                <td>{{ appointment.price }}</td>
                <td>{{ appointment.notes }}</td>
            </tr>
            </tbody>
        </table>
    </div>
</template>

<style scoped>
table {
    width: 100%;
    border-collapse: separate;
    border-spacing: 0 10px;
}

th, td {
    padding: 15px;
    text-align: left;
    border-bottom: 1px solid #ddd;
}

tr:hover {background-color: #f5f5f5;}
</style>