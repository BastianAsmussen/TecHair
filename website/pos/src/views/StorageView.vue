<script >
import FetchProduct from '@/components/FetchProductComponent.vue';

export  default {
    name: 'StorageView',
    components: {
        FetchProduct
    },
    data() {
        return {
            latestPrice: (priceHistory) => {
                return priceHistory.reduce((latest, time) => {
                    return latest.time > time.time ? latest : time;
                }).value;
            },
            products: [],
            selectedProducts: [],
        }
    },
    methods: {
        handleProductsFetched(products) {
            this.products = products;
        },
        addProductToSales(product) {
            this.selectedProducts.push(product);
        }
    },
}

</script>

<template>
    <FetchProduct @products-fetched="handleProductsFetched" />
    <h1 class="page-title">Products</h1>
    <button @click="$router.push('/')">Home</button>
    <div class="grid-item" v-for="product in products" :key="product.id" @click="addProductToSales(product)">
        <img :src="product.image" alt="product image" width="50" height="50">
        <p> name: {{ product.name }}</p>
        <p> price: {{ latestPrice(product.priceHistory) }}</p>
        <p> description: {{ product.description }}</p>
        <p> stock: {{product.stock}}</p>
    </div>

</template>

<style scoped>
.page-title {
    font-size: 2em;
    text-align: center;
    color: #333;
    margin-bottom: 20px;
}

button {
    display: block;
    margin: 10px auto;
    padding: 10px 20px;
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 1em;
}

button:hover {
    background-color: #0056b3;
}

.grid-item {
    display: inline-block;
    width: calc(31.9% - 20px); /* Adjust the width for a 3-column layout, considering the margin */
    background-color: #f9f9f9;
    border: 1px solid #eee;
    margin: 10px;
    padding: 10px;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    transition: transform 0.3s ease;
}

.grid-item:hover {
    transform: translateY(-5px);
    box-shadow: 0 4px 8px rgba(0,0,0,0.2);
}

.grid-item img {
    width: 100%; /* Make the image responsive */
    height: auto;
    border-radius: 5px;
}

.grid-item p {
    margin: 5px 0;
    color: #333;
}
</style>
```
