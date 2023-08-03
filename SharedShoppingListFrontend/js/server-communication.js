const api_url = "https://localhost:7206/api/";


function tryLogin() {
    let username, password;
    username = document.getElementById('username').value;
    password = document.getElementById('password').value;

    let data = {
        "username": username,
        "password": password
    }

    const endpoint = api_url + '/Auth/login';
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
        mode: 'cors'
    };

    fetch(endpoint, options)
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error('Something went wrong');
            }
        });
}