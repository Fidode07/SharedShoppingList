const api_url = "http://localhost:5133/api"; // if https is enabled in backend, it will automatically choose https
let alertBox;

function initAlertBox() {
    if (alertBox) return;
    alertBox = document.getElementById('alert-box');
}

function tryLogin() {
    let username, password;
    username = document.getElementById('username').value;
    password = document.getElementById('password').value;

    let data = {
        "username": username,
        "password": password
    }

    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
        mode: 'cors'
    };

    fetch(api_url + '/Auth/login', options)

        .then(response => {
            return response.json();
        }).then(data => {
        if (!data.success) {
            setAlertBox(generate_bootstrap_error('Login failed!', data.message));
            return;
        }
        setAlertBox('');
        userId = data.data;

        // Local Storage
        localStorage.setItem('userId', data.data);
        changeContainer('group-list');
        initGroups();
    });
}

function buildOptions(infos, method = 'POST') {
    if (method === 'GET' || method === 'HEAD') {
        return {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            mode: 'cors'
        };
    }
    return {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(infos),
        mode: 'cors'
    };
}

function tryRegister() {
    let username, pw, pw2;
    username = document.getElementById('username2').value;
    pw = document.getElementById('password2').value;

    if (username.length === 0 || pw.length === 0) {
        setAlertBox(generate_bootstrap_error('Registration failed!', 'Username or password is empty!'));
        return;
    }

    pw2 = document.getElementById('rp-password2').value;

    if (pw !== pw2) {
        setAlertBox(generate_bootstrap_error('Registration failed!', 'Passwords do not match!'));
        return;
    }

    let options = buildOptions({
        "username": username,
        "password": pw
    });

    fetch(api_url + '/Auth/register', options)
        .then(response => {
            return response.json();
        }).then(data => {
        if (!data.success) {
            setAlertBox(generate_bootstrap_error('Registration failed!', data.message));
            return;
        }
        setAlertBox('');
        userId = data.data;

        // Local Storage
        localStorage.setItem('userId', data.data);

        initGroups();
        changeContainer('group-list');
    });
}

function initGroups() {
    const options = buildOptions(null, 'GET');

    fetch(api_url + `/Group/get_groups?uniqueUserId=${userId}`, options)
        .then(response => {
            return response.json();
        }).then(data => {
        if (!data.success) {
            setAlertBox(generate_bootstrap_error('Error!', data.message));
            return;
        }
        setAlertBox('');
        let groups = data.data;
        let groupList = document.getElementById('group-list');
        groupList.innerHTML = '';
        groups.forEach(group => {
            groupList.innerHTML += new Group(group.id, group.name).getHtml();
        });
    });
}

function groupClick(groupId) {
    changeContainer('shopping-list-editor');
    // TODO: Fetch group data
    const options = buildOptions(null, 'GET');
    fetch(api_url + `/Group/get_group?groupId=${groupId}&uniqueUserId=${userId}`, options).then(response => {
        return response.json();
    }).then(data => {
        if (!data.Success) {
            setAlertBox(generate_bootstrap_error('Error!', data.message));
            return;
        }
        setAlertBox('');

        let shoppingListStr = data.Data.ShoppingList;
        let members = data.Data.Members.$values;
        let membersContainer = document.getElementById('group-member-list');

        // Put shopping list into shopping list editor
        let shoppingListEditor = document.getElementById('shopping-list-editor');
        shoppingListEditor.value = shoppingListStr;

        // Put members into member list
        membersContainer.innerHTML = '';
        members.forEach(member => {
            membersContainer.innerHTML += new User(member.User.Username).getHtml();
        });
    });
}

function setAlertBox(text) {
    initAlertBox();
    alertBox.innerHTML = text;
}

function generate_bootstrap_error(txt1, txt2) {
    return `<div class="alert alert-danger alert-dismissible fade show" role="alert">
              <strong>${txt1}</strong> ${txt2}
              <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>`;
}

function tryCreateGroup() {
    let groupName = document.getElementById('group-name').value;
    if (groupName.length === 0) {
        setAlertBox(generate_bootstrap_error('Error!', 'Group name is empty!'));
        return;
    }
    let description = document.getElementById('group-description').value;

    let options = buildOptions({
        "name": groupName,
        "description": description,
        "uniqueUserId": userId
    });

    fetch(api_url + '/Group/create_group', options)
        .then(response => {
            return response.json();
        }).then(data => {
        if (!data.success) {
            setAlertBox(generate_bootstrap_error('Error!', data.message));
            return;
        }
        setAlertBox('');
        initGroups();
        changeContainer('group-list');
    });
}

class Group {
    constructor(id, name) {
        this.id = id;
        this.name = name;
    }

    getHtml() {
        return `<div class="col-lg-12 group-card-container">
                    <div class="group-card" onclick="groupClick(${this.id});">
                        <h1 class="title">${this.name}</h1>
                    </div>
                </div>`;
    }
}

class User {
    constructor(name) {
        this.name = name;
    }

    getHtml() {
        return `<div class="col-lg-12 group-card-container">
                <div class="group-card">
                    <h1 class="title">${this.name}</h1>
                </div>
            </div>`;
    }
}