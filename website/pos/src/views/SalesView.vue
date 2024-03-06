<script>

</script>

<template>
    <div class="page">
        <div class="products">
            <h1 class="page-title">Products</h1>
            <div class="grid-container">
                <div class="grid-item">1</div>
                <div class="grid-item">2</div>
                <div class="grid-item">3</div>
                <div class="grid-item">4</div>
                <div class="grid-item">5</div>
                <div class="grid-item">6</div>
                <div class="grid-item">7</div>
                <div class="grid-item">8</div>
                <div class="grid-item">9</div>
            </div>
        </div>

        <div class="sales">
            <h1 class="page-title">Sales</h1>
            <div class="sales-products">
                <div class="sales-grid-container">
                    <div class="sales-grid-item">1</div>
                    <div class="sales-grid-item">2</div>
                </div>
            </div>

            <div class="sales-info">
                <div class="sales-prize">
                    <p>prize : </p>
                    <p> moms :</p>
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

}
.sales-grid-item {
    border: 1px solid black;
    text-align: center;
    padding: 20px;
    height: 5vh;
    margin: 10px;
    border-radius: 10px;
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