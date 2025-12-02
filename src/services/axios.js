import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:3000/api', // cambiar cuando tengas backend real
  timeout: 10000
})

export default api
