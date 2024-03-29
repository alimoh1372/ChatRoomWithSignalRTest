﻿var activeUserId = '';
var UserForm = document.getElementById('userDefine');
var userName = '';
var currentUserId = '';
var container = document.getElementById('toBlur');
var roomListEl = document.getElementById('roomList');
var roomHistoryEl = document.getElementById('chatHistory');
//Initial chatHubConnection
var chatConnection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();


chatConnection.onclose(function () {
    container.classList.add('blured');
    var enterYourNameWarningEl = document.getElementById('enterNameInfo');
    enterYourNameWarningEl.style.display = 'block';
    enterYourNameWarningEl.firstElementChild.text = "Disconnected From Server...";

    //Try to reconnect 5 time
    tryToReconnect();
});

function tryToReconnect() {
    var isError = false;
    var counter = 0;
    var error = '';
    while (isError || counter > 4) {
       
        setTimeout(function () {
                chatConnection.start()
                    .catch((err) => {
                        error = err;
                    });
            },
            5000);
        counter = counter + 1;
        isError = (error.length < 1);
    }
    if (isError) {
        alert(error+'\n Please call admin...');
    } else {
        openChatSegment();
    }
}
UserForm.addEventListener('submit', function (e) {
    e.preventDefault();
    const isSearchFormClass = UserForm.classList.contains('searchForm');

    if (isSearchFormClass) {
        //TODO: Add Search User Name from users operation

    } else {
        //first Time Enter button press
        userName = e.target[0].value;
        e.target[0].value = '';
        e.target[0].placeholder = 'Search Users...';
        UserForm.classList.add('searchForm');
        chatConnection.invoke('SetName', userName)
            .catch((err) => alert("An exception accord on server side..."));
        openChatSegment();
    }
});


//Handler of server request

chatConnection.on('responseSetName', setNameResult);
chatConnection.on('ReceiveMessage', addMessage);
chatConnection.on('loadUserNamesChatWithThem', loadChatList);
chatConnection.on('responseOfLoadChatHistory',addMessages)
// TODO: Initialize hub connections


////make a function to start connection 

//function S() {
//    var conresult = '';
//    chatConnection.start()
//        .catch(function (err) {
//            console.alert("!!!Atention Error:\n" + err);
//            conresult = err;
//        });
//    if (conresult.length > 0) {

//    } else {
//        openChatSegment();
//    }

//}

//make a handle connection when connection is disconnected
//chatConnection.onclose(function () {
//    handleDisconnected();
//});

//handle changes of page after connection started
function openChatSegment() {
    container.classList.remove('blured');
    var enterYourNameWarningEl = document.getElementById('enterNameInfo');
    enterYourNameWarningEl.style.display = 'none';
}

//public method give handler to retry connect again to server
//function handleDisconnected(retryFunc) {
//    container.classList.add('blured');
//    console.log('Reconnecting in 5 seconds...');
//    setTimeout(retryFunc, 5000);
//}

function setNameResult(result) {
    if (result) {
        //if the user exist before
        //ToDO: implement add history of chat
    } else {
        //if the user isn't exit before
        const message = 'Hi Dear ' + userName + ' welcome to the ChatRoom';
        const timeOffsetUtcNow = new Date().getTimezoneOffset(0);
        addMessage(0, 'Application', 0, currentUserId, userName, timeOffsetUtcNow, message);
    }
}

function loadChatList(userNameList, currentUserIdInpute) {
    
    if (!currentUserIdInpute)
        return;
    if (!userNameList.length)
        return;
    currentUserId = currentUserIdInpute;
    const userNames = JSON.parse(userNameList);

    loadRooms(userNames);
}

function sendMessage(text) {
    if (text && text.length) {
        // TODO: Send an agent message
        if (!currentUserId)
            return;
        if (!activeUserId)
            return

        chatConnection.invoke('SendMessage',currentUserId,activeUserId,text)
    }
}

function ready() {
    // TODO: Start the hub connections

    chatConnection.start();

    var chatFormEl = document.getElementById('chatForm');
    chatFormEl.addEventListener('submit', function (e) {
        e.preventDefault();

        var text = e.target[0].value;
        e.target[0].value = '';
        sendMessage(text);
    });
}

function switchActiveRoomTo(id) {
    if (id === activeUserId) return;

    if (activeUserId) {
        // TODO: Leave the room
    }

    activeUserId =Number(id);
    
    removeAllChildren(roomHistoryEl);

    if (!id) return;

    // TODO: Join the room

    // TODO: Load the room history
    activeUserIdNumber = Number(activeUserId);
    chatConnection.invoke('LoadChatHistory', currentUserId, activeUserIdNumber)
        .catch(err => {
            alert(err);
        });

}




roomListEl.addEventListener('click', function (e) {
    roomHistoryEl.style.display = 'block';

    setActiveRoomButton(e.target);

    var roomId = e.target.getAttribute('data-id');
    switchActiveRoomTo(roomId);
});

function setActiveRoomButton(el) {
    var allButtons = roomListEl.querySelectorAll('a.list-group-item');

    allButtons.forEach(function (btn) {
        btn.classList.remove('active');
    });

    el.classList.add('active');
}

function loadRooms(rooms) {
    if (!rooms.length) return;


    switchActiveRoomTo(null);
    removeAllChildren(roomListEl);

    for (var i = 0; i < rooms.length; i++) {
        var item = rooms[i];
        var currentId = item.Id;
        var currentName = item.UserName;
        if (!currentName) continue;

        var roomButton = createRoomButton(currentId, currentName);
        roomListEl.appendChild(roomButton);
    }
    //rooms.forEach(function (item) {
    //    var currentId = item.id;
    //    var currentName = item.name;
    //    if (!currentName) return;

    //    var roomButton = createRoomButton(currentId, currentName);
    //    roomListEl.appendChild(roomButton);
    //});
}

function createRoomButton(id, userName) {
    var anchorEl = document.createElement('a');
    anchorEl.className = 'list-group-item list-group-item-action d-flex justify-content-between align-items-center';
    anchorEl.setAttribute('data-id', id);
    anchorEl.textContent = userName;
    anchorEl.href = '#';

    return anchorEl;
}

function addMessages(messages) {
   
    if (!messages) return;
  
    
    messages.forEach(function (m) {
        
        const id = m.id;
        const senderId = m.fkSenderUserId;
        const senderName = m.fkSenderUserName;
        const receiverId = m.fkReceiverUserid;
        const receiverName = m.fkReceiverUserName;
        const creationTimeStamp = m.creationDate;
        const messageContent = m.messageContent;
        addMessage(m.Id, senderId, senderName, receiverId, receiverName, creationTimeStamp, messageContent);
    });
}

function addMessage(id,senderId,senderName,receiverId,receiverName, time, message) {
    var nameSpan = document.createElement('span');
    nameSpan.className = 'name';
    nameSpan.setAttribute('data-id', senderId);
    nameSpan.textContent = senderName;

    var recieverNameEl = document.createElement('span');
    recieverNameEl.className = 'name';
    recieverNameEl.textContent = receiverName;
    recieverNameEl.setAttribute('data-id', receiverId);

    var timeSpan = document.createElement('span');
    timeSpan.className = 'time';
    var friendlyTime = moment(time).fromNow();
    timeSpan.textContent = friendlyTime;

    var headerDiv = document.createElement('div');
    headerDiv.appendChild(nameSpan);
    var textNode = document.createTextNode(" SaidTo ");
    headerDiv.appendChild(textNode);
    headerDiv.appendChild(recieverNameEl);
    headerDiv.appendChild(timeSpan);

    var messageDiv = document.createElement('div');
    messageDiv.className = 'message';
    messageDiv.setAttribute('data-id', id);
    messageDiv.textContent = message;


    var newItem = document.createElement('li');
    newItem.appendChild(headerDiv);
    newItem.appendChild(messageDiv);

    roomHistoryEl.appendChild(newItem);
    roomHistoryEl.scrollTop = roomHistoryEl.scrollHeight - roomHistoryEl.clientHeight;
}

function removeAllChildren(node) {
    if (!node) return;

    while (node.lastChild) {
        node.removeChild(node.lastChild);
    }
}

document.addEventListener('DOMContentLoaded', ready);