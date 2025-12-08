import { createApp } from 'vue'
import App from './App.vue'
import './style.css'
import router from './router'
import { createPinia } from 'pinia'
import { plugin, defaultConfig } from '@formkit/vue'


const app = createApp(App)
app.use(createPinia())
app.use(plugin, defaultConfig({
    config:{
        classes:{
             message: "text-red-500 text-sm mt-1 font-bold"
        }
    }
}))
app.use(router)
app.mount('#app')
