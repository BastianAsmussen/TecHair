<script>
import FetchProduct from '@/components/FetchProductComponent.vue';

export default {
    name: 'SalesView',
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
    computed: {
        totalPrice() {
            return this.selectedProducts.reduce((total, product) => {
                return total + this.latestPrice(product.priceHistory);
            }, 50);
        }
    },
    methods: {
        handleProductsFetched(products) {
            // Filter products to include only those with stock greater than 0
            this.products = products.filter(product => product.stock > 0);
        },
        addProductToSales(product) {
            this.selectedProducts.push(product);
        }
    },
}
</script>

<template>
    <div class="page">
        <div class="products">
            <h1 class="page-title">Products</h1>
            <button @click="$router.push('/')">Home</button>
            <div class="grid-container">
                <FetchProduct @products-fetched="handleProductsFetched" />
                <div class="grid-item" v-for="product in products" :key="product.id"  @click="addProductToSales(product)">
                    <div v-if="product.stock > 0">
                        <img :src="product.image" alt="product image" width="50" height="50">
                        <p>{{ product.name }}</p>
                        <p>{{ latestPrice(product.priceHistory) }}</p>
                        <p>{{ product.description }}</p>
                    </div>
                </div>
            </div>
        </div>

        <div class="sales">
            <h1 class="page-title">Sales</h1>
            <div class="sales-products">
                <div class="sales-grid-container">
                    <div class="sales-grid-item" v-for="product in selectedProducts" :key="product.id">
                        <p>{{ product.name }}</p>
                        <p>Price: {{ latestPrice(product.priceHistory) }}</p>
                    </div>
                </div>
            </div>

            <div class="sales-info">
                <div class="sales-prize">
                    <p>Price: {{ totalPrice }}kr</p>
                    <p>Moms:  </p>
                </div>
                <button class="confirm-btn">
                    <p>confirm order</p>
                </button>
            </div>
        </div>
    </div>
</template>



<style scoped>
.page{
    display: flex;
    justify-content: space-between;
    background: #DEE2E6;
}

.products{
    flex: 0 0 70%;
    height: 100vh;
    border-right: 1px solid #a7a7a7;


}

.grid-container {
    display: grid;
    grid-template-columns: auto auto auto;
    gap: 10px;
    padding: 10px;
}
.grid-item {
    border: 1px solid black;
    text-align: center;
    padding: 20px;
    height: 15vh;
    border-radius: 10px;
}
.grid-item:hover {
    background: #F8F9FA;
}
.page-title {
    text-align: center;
    margin-top: 20px;
}

.sales {
    background: #DEE2E6;
    flex: 0 0 30%;
    border-radius: 5px;
    box-shadow: -2px 0px 5px 0px #a7a7a7
}
.sales-products {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    height: 70%;
    background: #F8F9FA;
    border-top: 1px solid ;
}
.sales-grid-container{
    display: grid;
    gap: 10px;
    padding: 10px;
    overflow: auto;
    max-height: 600px;


}
.sales-grid-item {
    border: 1px solid black;
    text-align: center;
    padding: 20px;
    height: 5vh;
    margin: 10px;
    border-radius: 10px;
    display: flex;
    justify-content: space-between;
}

.sales-info{
    background: #DEE2E6 ;
}

.sales-prize{
    border-top: 1px solid ;
}

.confirm-btn {
    bottom: 0;
    font-size: 20px;
    width: 90%;
    margin-left: 5%;
}

</style>