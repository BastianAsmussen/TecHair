import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import SalesView from '@/views/SalesView.vue'
import AppointmentsView from "@/views/AppointmentsView.vue";
import StorageView from "@/views/StorageView.vue";

const routes = [
    {
        path: '/',
        name: 'Home',
        component: HomeView
    },
    {
        path: '/sales',
        name: 'Sales',
        component: SalesView
    },
    {
        path: '/appointments',
        name: 'Appointments',
        component: AppointmentsView
    },
    {
        path: '/storage',
        name: 'Storage',
        component: StorageView
    }
]


const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router