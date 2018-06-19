$(function () {

    // Declare a proxy to reference the hub.
    var chatHub = $.connection.chathub;

    registerClientMethods(chatHub);

    // Start Hub
    $.connection.hub.start().done(function () {
        registerEvents(chatHub);
    });

});

//Adiciona os eventos de enviar mensagem do cliente
function registerEvents(chatHub) {
    
    var userEmail = $("#hdEmail").val();
    var userId = $("#hdId").val();
    chatHub.server.connect(userEmail, userId);
        
    $('#btnSendMsg').click(function () {

        var msg = $("#txtMessage").val();
        var idConversa = $('#hdIdConversaAtual').val();
        if (msg.length > 0 && idConversa != "") {
            var userName = $('#hdUserName').val();
            alert("FOIIIII");
            chatHub.server.sendMessageToContato(userName, msg);
            $("#txtMessage").val('');
        }
    });
    
    $("#txtMessage").keypress(function (e) {
        if (e.which == 13) {
            $('#btnSendMsg').click();
        }
    });

    //Função onClick que adiciona o contato
    $('#addCont').click(function () {

        var emailContato = $("#emailContato").val();
        if (emailContato.length > 0) {
            var idUser = $('#hdId').val();
            chatHub.server.addContato(emailContato, idUser);
            $("#emailContato").val('');
        }
    });

    $("#divusers").on("click", "a", function () {
        var idCont = $(this).find("input[type='hidden']").attr('id');
        var idUser = $('#hdId').val();
        chatHub.server.getMessagesByConversa(idUser, idCont);
    });
}

function registerClientMethods(chatHub) {

    // Calls when user successfully logged in
    chatHub.client.onConnected = function (connectionID, userEmail, userContatos) {

        $('#hdConnectionId').val(connectionID);

        // Add Contatos na lista de contatos
        for (i = 0; i < userContatos.length; i++) {
            PutContato(chatHub, userContatos[i].UserName, userContatos[i].IdUser);
        }
    }

    // On New User Connected
    chatHub.client.onNewUserConnected = function (id, name) {
        AddUser(chatHub, id, name);
    }

    // On User Disconnected
    chatHub.client.onUserDisconnected = function (id, userName) {

        $('#' + id).remove();

        var ctrId = 'private_' + id;
        $('#' + ctrId).remove();


        var disc = $('<div class="disconnect">"' + userName + '" logged off.</div>');

        $(disc).hide();
        $('#divusers').prepend(disc);
        $(disc).fadeIn(200).delay(2000).fadeOut(200);

    }

    // chama o metodo no cliente q adiciona a mensagem na div 
    chatHub.client.messageReceived = function (userName, message) {
        AddMessage(userName, message);
    }

    chatHub.client.sendPrivateMessage = function (windowId, fromUserName, message) {

        var ctrId = 'private_' + windowId;


        if ($('#' + ctrId).length == 0) {

            createPrivateChatWindow(chatHub, windowId, ctrId, fromUserName);

        }

        $('#' + ctrId).find('#divMessage').append('<div class="message"><span class="userName">' + fromUserName + '</span>: ' + message + '</div>');

        // set scrollbar
        var height = $('#' + ctrId).find('#divMessage')[0].scrollHeight;
        $('#' + ctrId).find('#divMessage').scrollTop(height);

    }

    chatHub.client.sendMessageToContato = function (fromUserName, message) {

    }

    chatHub.client.sendMessageToGrupo = function (windowId, fromUserName, message) {
        
    }

    //Função que adiciona usuario na lista de contatos 
    chatHub.client.addContatoLista = function (nome,idConversa) {
        PutContato(chatHub, nome, idConversa);
    }

    //Função chamada pelo server que passa um array de mensagens de uma conversa 
    chatHub.client.addMessagesConversa = function (mensagens, idConversa) {
        $('#divChatWindow').html("");
        $('#hdIdConversaAtual').val(idConversa);
        //Adiciona as mensagens na div
        for (i = 0; i < mensagens.length; i++) {
            AddMessage(mensagens[i].UserName, mensagens[i].Mensagem);
        }
    }
}

function PutContato(chatHub, name, idCont) {

    var code = $('<a href="#" id="ContConversa" class="list-group-item">' + name + '<input type="hidden" id="' + idCont + '" ></a>');
    $("#divusers").append(code);
}

function AddUser(chatHub, id, name) {

    var userId = $('#hdConnectionId').val();

    var code = "";

    if (userId == id) {

        code = $('<div class="loginUser">' + name + "</div>");

    } else {

        code = $('<a id="' + id + '" class="user" >' + name + '<a>');

        $(code).dblclick(function () {

            var id = $(this).attr('id');

            if (userId != id)
                OpenPrivateChatWindow(chatHub, id, name);

        });
    }

    $("#divusers").append(code);

}

//Função que adiciona mensagem na div do chat
function AddMessage(userName, message) {
    $('#divChatWindow').append('<li class="other"><div class="msg"><div class="user">' + userName + '</div><p>' + message + '</p></div></li>');
    
    var height = $('#divChatWindow')[0].scrollHeight;
    $('#divChatWindow').scrollTop(height);
}