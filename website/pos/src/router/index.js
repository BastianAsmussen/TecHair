import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '@/views/LoginView.vue'
import HomeView from '@/views/HomeView.vue'
import SalesView from '@/views/SalesView.vue'
import AppointmentsView from "@/views/AppointmentsView.vue";
import StorageView from "@/views/StorageView.vue";

const routes = [
    {
        path: '/',
        name: 'Login',
        component: LoginView
    },
    {
        path: '/',
        name: 'Home',
        component: HomeView,
        meta: { requiresAuth: true }
    },
    {
        path: '/sales',
        name: 'Sales',
        component: SalesView,
        meta: { requiresAuth: true }
    },
    {
        path: '/appointments',
        name: 'Appointments',
        component: AppointmentsView,
        meta: { requiresAuth: true }
    },
    {
        path: '/storage',
        name: 'Storage',
        component: StorageView,
        meta: { requiresAuth: true }
    }
]


const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router