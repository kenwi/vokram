var ViewportContainer = React.createClass({
    getInitialState: function () {
        return { data: [] };
    },

    render: function () {
        return (
            <div id="viewport">
                <SidebarContainer data={this.state.data} />

                <FooterContainer />
                <div id="main">
                    <div id="windows">
                        <ChatContainer data={this.state.data} />
                    </div>
                </div>
            </div>);
    }
});


var WindowsContainer = React.createClass({
    render: function () {
        return (
            <div id="windows">
                <ChatContainer />
            </div>);
    }
});

var FooterContainer = React.createClass({
    render: function () {
        return (
            <footer id="footer">
                <span className="tooltipped tooltipped-n" aria-label="Sign in"><button className="icon sign-in" data-target="#sign-in" aria-label="Sign in"></button></span>
                <span className="tooltipped tooltipped-n" aria-label="Connect to network"><button id="connect" className="icon connect" data-target="#connect" aria-label="Connect to network"></button></span>
                <span className="tooltipped tooltipped-n" aria-label="Client settings"><button className="icon settings" data-target="#settings" aria-label="Client settings"></button></span>
                <span className="tooltipped tooltipped-n" aria-label="Sign out"><button className="icon sign-out" id="sign-out" aria-label="Sign out"></button></span>
            </footer>);
    }
});

var ShowOlderMessagesButton = React.createClass({
    render: function () {
        return (
            <div className="chat">
                <div className="show-more "><button className="show-more-button" >Show older messages</button></div>
            </div>);
    }
});

var ChannelHeader = React.createClass({
    render: function () {
        return (
            <div className="header">
                <button className="lt" aria-label="Toggle channel list"></button>
                <button className="menu" aria-label="Open the context menu"></button>
                <span className="title">{this.props.Topic}</span>
                <span title="" className="topic"></span>
            </div>);
    }
});

var ChannelMessage = React.createClass({
    render: function () {
        return (
            <div className="msg mode self">
                <span className="time">{this.props.Time}</span>
                <span className="from"></span>
                <span className="text"><span role="button" className="user color-31">{this.props.From}</span> {this.props.Text}</span>
            </div>);
    }
});

var InputContainer = React.createClass({
    render: function () {
        return (
            <form id="form" method="post" action="">
                <div className="input">

                    <span id="nick">
                        <span id="nick-value" spellcheck="false" contenteditable="false">m0b_</span>
                        <span id="set-nick-tooltip" className="tooltipped tooltipped-e" aria-label="Change nick"><button id="set-nick" type="button" aria-label="Change nick"></button></span>
                        <span id="cancel-nick-tooltip" className="tooltipped tooltipped-e" aria-label="Cancel"><button id="cancel-nick" type="button" aria-label="Cancel"></button></span>
                        <span id="save-nick-tooltip" className="tooltipped tooltipped-e" aria-label="Save"><button id="submit-nick" type="button" aria-label="Save"></button></span>
                    </span>

                    <input type="text" id="input" className="mousetrap" placeholder="Write to #nff" />

                    <span id="cycle-nicks-tooltip" className="tooltipped tooltipped-w" aria-label="Cycle through nicks">
                        <button id="cycle-nicks" type="button" aria-label="Cycle through nicks"></button>
                    </span>

                    <span id="submit-tooltip" className="tooltipped tooltipped-w" aria-label="Send message">
                    </span>
                </div>
            </form>);
    }
});



createMessage = (nick, message) => {
    return ' \
                <div class="msg mode self"> \
                    <span class="time">'+ nick.split(' ')[0] + '</span> \
                    <span class="from"></span> \
                    <span class="text"><span role="button" class="user color-31">' + nick.split(' ')[1] + '</span> ' + message + '</span> \
                </div > ';
};

htmlEncode = (value) => {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

var ViewportContainer = React.createClass({
    getInitialState: function () {
        return { data: this.props.data };
    },

    componentDidMount: function () {
        //this.setState({ data: channels });
    },

    render: function () {
        return (
            <div id="viewport">
                <SidebarContainer data={this.state.data} />

                <FooterContainer />
                <div id="main">
                    <div id="windows">
                        <ChatContainer data={this.state.data} />
                    </div>
                </div>
            </div>);
    }
});

var SidebarContainer = React.createClass({
    render: function () {
        var nodes = this.props.data.map(function (channel) {
            return (
                <SidebarTextElement Text={channel.Name} Style="chan channel" />);
        });

        return (
            <aside id="sidebar">
                <div className="networks">
                    <section className="network">
                        {nodes}
                    </section>
                </div>
            </aside>);
    }
});

var SidebarTextElement = React.createClass({
    render: function () {
        return (
            <div className={this.props.Style}>
                <span className="badge"></span>
                <button className="close" aria-label="Close"></button>
                <span className="name" title="Freenode">{this.props.Text}</span>
            </div>);
    }
});

var ChatContainer = React.createClass({
    render: function () {
        return (
            <div id="chat-container" className="window active">
                <ChannelContainer data={this.props.data} />
                <InputContainer />
            </div>);
    }
});

var ChannelContainer = React.createClass({
    render: function () {
        var nodes = this.props.data.map(function (channel) {
            return (
                <MessagesContainer data={channel.Messages} />);
        });

        return (
            <div id="chat">
                <div className="chan channel active">
                    <ChannelHeader Topic="#hadamard - Hello world, Topic2" />
                    <ShowOlderMessagesButton />
                    {nodes}
                </div>
            </div>);
    }
});

var MessagesContainer = React.createClass({
    render: function () {
        var nodes = this.props.data.map(function (message) {
            return (
                <ChannelMessage Time={message.Time} From={message.From} Text={message.Text} />);
        });

        return (
            <div id="messages" className="messages">
                <div className="unread-marker">
                    <span className="unread-marker-text"></span>
                </div>

                {nodes}
            </div>);
    }
});

var channels = [

    {
        Name: "#hadamard",
        Messages: [
            {
                Time: "10:10",
                From: "m0b",
                Text: "Hello, World!"
            }
        ]
    }
];


var messages = [];
$(() => {
    var chat = $.connection.chatHub;

    chat.client.addNewMessageToPage = (name, message) => {
        var message = createMessage(name, message);
        var channel = channel.filter(function (e) {
            return e.Name == "#hadamard";
        });
        channel.Messages.push(message);
    };

    $.connection.hub.start().done(() => {
        $('#connect').click(() => {
            chat.server.connect('irc.freenode.net');
        });
    });
});

ReactDOM.render(
    <ViewportContainer data={channels} />,
    document.getElementById('viewport')
);
