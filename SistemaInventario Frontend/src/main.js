import { createApp } from 'vue'
import App from './App.vue'
import './style.css'
import router from './router'
import { createPinia } from 'pinia'
import { plugin, defaultConfig } from '@formkit/vue'

const app = createApp(App)
app.use(createPinia())
app.use(
  plugin,
  defaultConfig({
    locales: {
      es: {
        ui: {
          incomplete: "Por favor complete todos los campos correctamente."
        }
      }
    },
    locale: "es", // activa la locale por defecto
    config: {
      classes: {
        message: "text-red-500 text-sm mt-1 font-bold"
      },
      form: {
        generateMessage: ({ node }) => {
          if (node.props.type === "form") {
            return "Por favor complete todos los campos correctamente.";
          }
        },
      }
    }
  })
)
app.use(router)
app.mount('#app')