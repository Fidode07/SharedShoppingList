const api_url = "https://localhost:7206/api";
let userId, clickedGroupId;


function tryLogin() {
    // let username, password;
    // username = document.getElementById('username').value;
    // password = document.getElementById('password').value;
    //
    // let data = {
    //     "username": username,
    //     "password": password
    // }
    //
    // const endpoint = api_url + '/Auth/login';
    // const options = {
    //     method: 'POST',
    //     headers: {
    //         'Content-Type': 'application/json'
    //     },
    //     body: JSON.stringify(data),
    //     mode: 'cors'
    // };
    //
    // fetch(endpoint, options)
    //     .then(response => {
    //         if (response.ok) {
    //             console.log('Login successful');
    //             return response.json();
    //         } else {
    //             throw new Error('Something went wrong');
    //         }
    //     });
    userId = 1;
    changeContainer('group-list');
    // TODO: Fetch Groups
}

function tryRegister() {
    let username, pw, pw2;
    username = document.getElementById('username2').value;
    pw = document.getElementById('password2').value;
    pw2 = document.getElementById('rp-password2').value;

    if (pw !== pw2) {
        alert('Passwords do not match!');
        return;
    }
    userId = 1;
    changeContainer('group-list');
}

function groupClick(groupId) {
    changeContainer('shopping-list-editor');
    // TODO: Fetch Shopping List and members of group [INSERT INTO DIVS]
}