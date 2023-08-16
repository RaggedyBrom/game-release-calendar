import { useState, useEffect } from 'react'
import './App.css'

function App() {

    const [state, setState] = useState({ result: [] })

    useEffect(() => {
        fetch(import.meta.env.VITE_API_BASE_URL + "/games/example")
            .then(resp => resp.json()
                .then(data => {

                    setState({ result: data })
                    console.log(data)
                })
            )
    })

return (
        <>
            <p>
                Id = {state.result.id}, Text = {state.result.text}
            </p>
        </>
    )
}

export default App
