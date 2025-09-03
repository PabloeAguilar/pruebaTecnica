<template>
  <div class="app-container">
    <header class="header">
      <h1>Sales Management System</h1>
    </header>

    <div class="main-content">
      <!-- Read-only Grids Section -->
      <div class="grids-section">
        <div class="grid-container">
          <h2>Customers</h2>
          <div class="table-wrapper">
            <table class="data-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Name</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="customer in customers" :key="customer.id">
                  <td>{{ customer.id }}</td>
                  <td>{{ customer.name }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div class="grid-container">
          <h2>Products</h2>
          <div class="table-wrapper">
            <table class="data-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Name</th>
                  <th>Price</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="product in products" :key="product.id">
                  <td>{{ product.id }}</td>
                  <td>{{ product.name }}</td>
                  <td>${{ product.list_price }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>

      <!-- Sale Form and Breakdown Section -->
      <div class="sale-section">
        <div class="sale-form-container">
          <h2>Create Sale</h2>
          <form @submit.prevent="submitSale" class="sale-form">
            <div class="form-row">
              <div class="form-group">
                <label for="customer">Customer:</label>
                <select id="customer" v-model="saleForm.customerId" required class="form-control">
                  <option value="">Select Customer</option>
                  <option v-for="customer in customers" :key="customer.id" :value="customer.id">
                    {{ customer.name }}
                  </option>
                </select>
              </div>

              <div class="form-group">
                <label for="paymentMethod">Payment Method:</label>
                <select id="paymentMethod" v-model="saleForm.paymentMethod" required class="form-control">
                  <option value="">Select Payment Method</option>
                  <option value="cash">Cash</option>
                  <option value="store credit">Store credit</option>
                </select>
              </div>
            </div>

            <div class="items-section">
              <h3>Items</h3>
              <div class="items-table-wrapper">
                <table class="items-table">
                  <thead>
                    <tr>
                      <th>Product</th>
                      <th>Quantity</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(item, index) in saleForm.items" :key="index">
                      <td>
                        <select v-model="item.productId" required class="form-control">
                          <option value="">Select Product</option>
                          <option v-for="product in products" :key="product.id" :value="product.id">
                            {{ product.name }} - ${{ product.list_price }}
                          </option>
                        </select>
                      </td>
                      <td>
                        <input type="number" v-model.number="item.quantity" min="1" required
                          class="form-control quantity-input" />
                      </td>
                      <td>
                        <button type="button" @click="removeItem(index)" class="btn btn-danger btn-sm"
                          :disabled="saleForm.items.length <= 1">
                          Remove
                        </button>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>

              <button type="button" @click="addItem" class="btn btn-secondary">
                Add Row
              </button>
            </div>

            <div class="form-actions">
              <button type="submit" class="btn btn-primary" :disabled="!isFormValid || isSubmitting">
                {{ isSubmitting ? 'Submitting...' : 'Submit Sale' }}
              </button>
            </div>
          </form>
        </div>

        <!-- Breakdown Panel -->
        <div class="breakdown-container" v-if="saleBreakdown">
          <h2>Sale Breakdown</h2>
          <div class="breakdown-content">
            <div class="breakdown-items">
              <h3>Items</h3>
              <ul class="items-list">
                <li v-for="line in saleBreakdown.breakdown.lines" :key="line.id || line.product_id">
                  {{ getProductName(line.product_id) }} -
                  Qty: {{ line.quantity }} × ${{ line.list_price }} = ${{ line.quantity * line.list_price }}
                </li>
              </ul>
            </div>

            <div class="breakdown-totals">
              <div class="total-row">
                <span>Subtotal:</span>
                <span>${{ saleBreakdown.subtotal }}</span>
              </div>
              <div class="total-row">
                <span>Tax:</span>
                <span>${{ saleBreakdown.tax }}</span>
              </div>
              <div class="total-row" v-if="saleBreakdown.total_discounts_amount > 0">
                <span>Total Discounts:</span>
                <span>-${{ saleBreakdown.total_discounts_amount }}</span>
              </div>
              <div class="total-row final-total">
                <span>Total:</span>
                <span>${{ saleBreakdown.total }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Loading and Error States -->
    <div v-if="loading" class="loading">Loading...</div>
    <div v-if="error" class="error">{{ error }}</div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'

// Types
interface Customer {
  id: number
  name: string
}

interface Product {
  id: number
  name: string
  list_price: number
}

interface SaleItem {
  productId: number | string
  quantity: number
}

interface SaleForm {
  customerId: number | string
  paymentMethod: string
  items: SaleItem[]
}

interface SaleBreakdown {
  breakdown: {
    lines: Array<{
      id?: number
      product_id: number
      quantity: number
      list_price: number
      total: number
    }>
  }
  subtotal: number
  tax: number
  total: number
  total_discounts_amount: number
}

// Reactive data
const customers = ref<Customer[]>([])
const products = ref<Product[]>([])
const loading = ref(false)
const error = ref('')
const isSubmitting = ref(false)
const saleBreakdown = ref<SaleBreakdown | null>(null)

const saleForm = ref<SaleForm>({
  customerId: '',
  paymentMethod: '',
  items: [{ productId: '', quantity: 1 }]
})

// API base URL
const API_BASE = 'http://localhost:5184'

// Computed properties
const isFormValid = computed(() => {
  return (
    saleForm.value.customerId !== '' &&
    saleForm.value.paymentMethod !== '' &&
    saleForm.value.items.length > 0 &&
    saleForm.value.items.every(item =>
      item.productId !== '' &&
      item.quantity > 0
    )
  )
})

// Methods
const loadCustomers = async () => {
  try {
    const response = await axios.get(`${API_BASE}/customers`)
    customers.value = response.data
  } catch (err) {
    console.error('Error loading customers:', err)
    error.value = 'Failed to load customers'
  }
}

const loadProducts = async () => {
  try {
    const response = await axios.get(`${API_BASE}/products`)
    products.value = response.data
  } catch (err) {
    console.error('Error loading products:', err)
    error.value = 'Failed to load products'
  }
}

const addItem = () => {
  saleForm.value.items.push({ productId: '', quantity: 1 })
}

const removeItem = (index: number) => {
  if (saleForm.value.items.length > 1) {
    saleForm.value.items.splice(index, 1)
  }
}

const getProductName = (productId: number) => {
  const product = products.value.find(p => p.id === productId)
  return product ? product.name : 'Unknown Product'
}

const submitSale = async () => {
  if (!isFormValid.value) return

  isSubmitting.value = true
  error.value = ''

  try {
    const saleData = {
      customer_id: Number(saleForm.value.customerId),
      payment_method: saleForm.value.paymentMethod,
      items: saleForm.value.items.map(item => ({
        product_id: Number(item.productId),
        quantity: item.quantity
      }))
    }

    const response = await axios.post(`${API_BASE}/sales`, saleData)
    saleBreakdown.value = response.data
    // Reset form
    saleForm.value = {
      customerId: '',
      paymentMethod: '',
      items: [{ productId: '', quantity: 1 }]
    }

  } catch (err) {
    console.error('Error submitting sale:', err)
    error.value = 'Failed to submit sale'
  } finally {
    isSubmitting.value = false
  }
}

// Load data on component mount
onMounted(async () => {
  loading.value = true
  try {
    await Promise.all([loadCustomers(), loadProducts()])
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.app-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
}

.header {
  text-align: center;
  margin-bottom: 30px;
}

.header h1 {
  color: #2c3e50;
  margin: 0;
}

.main-content {
  display: flex;
  flex-direction: column;
  gap: 30px;
}

.grids-section {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.grid-container {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  padding: 20px;
}

.grid-container h2 {
  margin: 0 0 15px 0;
  color: #34495e;
  font-size: 1.5em;
}

.table-wrapper {
  overflow-x: auto;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}

.data-table th,
.data-table td {
  padding: 12px;
  text-align: left;
  border-bottom: 1px solid #ddd;
  color: #000;
}

.data-table th {
  background-color: #f8f9fa;
  font-weight: 600;
  color: #495057;
}

.data-table tbody tr:hover {
  background-color: #f8f9fa;
}

.sale-section {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 20px;
}

.sale-form-container,
.breakdown-container {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  padding: 20px;
}

.sale-form-container h2,
.breakdown-container h2 {
  margin: 0 0 20px 0;
  color: #34495e;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
  margin-bottom: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group label {
  margin-bottom: 5px;
  font-weight: 600;
  color: #495057;
}

.form-control {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  transition: border-color 0.3s;
}

.form-control:focus {
  outline: none;
  border-color: #007bff;
  box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
}

.items-section {
  margin-bottom: 20px;
}

.items-section h3 {
  margin: 0 0 15px 0;
  color: #495057;
}

.items-table-wrapper {
  overflow-x: auto;
  margin-bottom: 15px;
}

.items-table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 10px;
}

.items-table th,
.items-table td {
  padding: 10px;
  border: 1px solid #ddd;
}

.items-table th {
  color: #000;
  background-color: #f8f9fa;
  font-weight: 600;
}

.quantity-input {
  width: 80px;
}

.btn {
  padding: 10px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  transition: all 0.3s;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background-color: #007bff;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background-color: #0056b3;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background-color: #545b62;
}

.btn-danger {
  background-color: #dc3545;
  color: white;
}

.btn-danger:hover:not(:disabled) {
  background-color: #c82333;
}

.btn-sm {
  padding: 5px 10px;
  font-size: 12px;
}

.form-actions {
  text-align: right;
}

.breakdown-content {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.breakdown-items h3 {
  margin: 0 0 10px 0;
  color: #495057;
}

.items-list {
  list-style: none;
  padding: 0;
  margin: 0;
  color: #000;
}

.items-list li {
  padding: 8px 0;
  border-bottom: 1px solid #eee;
}

.breakdown-totals {
  border-top: 2px solid #ddd;
  padding-top: 15px;
}

.total-row {
  display: flex;
  justify-content: space-between;
  padding: 5px 0;
  font-size: 14px;
  color: #000;
}

.final-total {
  font-weight: bold;
  font-size: 16px;
  color: #2c3e50;
  border-top: 1px solid #ddd;
  padding-top: 10px;
  margin-top: 10px;
}

.loading,
.error {
  text-align: center;
  padding: 20px;
  margin: 20px 0;
  border-radius: 4px;
}

.loading {
  background-color: #d1ecf1;
  color: #0c5460;
}

.error {
  background-color: #f8d7da;
  color: #721c24;
}

@media (max-width: 768px) {

  .grids-section,
  .sale-section {
    grid-template-columns: 1fr;
  }

  .form-row {
    grid-template-columns: 1fr;
  }
}
</style>