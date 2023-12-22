const btn = document.getElementById("button")
const btnDevTools = document.getElementById("buttonDevTools")

btn.onclick = () => {
    alert("Hello from javascript 😁")
}

btnDevTools.onclick = () => {
    alert("showDevTools")
}

document.addEventListener('keydown', evt => {
    console.log("key", evt)
    evt.preventDefault()
    evt.stopPropagation()
})