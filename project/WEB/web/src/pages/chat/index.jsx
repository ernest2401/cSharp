//React
import React from 'react';

import * as signalR from '@aspnet/signalr';

import { API_HOST } from '../../resources/config';
import { Input, Button } from 'antd';
import LayoutPage from '../layout';

class ChatPage extends React.Component {
    constructor(props) {
        super(props);

        this.connection = null;
        this.state = {
            currentMessage: "",
            messages: []
        };
    }

    componentDidMount() {
        const protocol = new signalR.JsonHubProtocol();

        const transport = signalR.HttpTransportType.WebSockets;

        const options = {
            transport,
            logMessageContent: true,
            logger: signalR.LogLevel.Trace,
            accessTokenFactory: () => this.props.accessToken,
        };

        // create the connection instance
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(`${API_HOST}/chatHub`, options)
            .withHubProtocol(protocol)
            .build();

        this.connection.on('ReceiveMessage', this.onNotifReceived);

        this.connection.start()
            .then(() => console.info('SignalR Connected'))
            .catch(err => console.error('SignalR Connection Error: ', err));
    }

    componentWillUnmount() {
        this.connection.stop();
    }

    onNotifReceived = (userName, message) => {
        this.setState({
            ...this.state,
            currentMessage: "",
            messages: [
                ...this.state.messages,
                { userName, message }
            ]
        });

        window.scrollTo(0,document.body.scrollHeight);
    }

    handleSendMessage = () => {
        const firstName = JSON.parse(localStorage.getItem("user_data")).firstName;
        const lastName = JSON.parse(localStorage.getItem("user_data")).lastName;

        this.connection.invoke("SendMessage", `${firstName} ${lastName}`, this.state.currentMessage);
    }

    render() {
        return (
            <LayoutPage>
                <h1 style={{ textAlign: 'center', marginTop: '25px' }}>Chat</h1>
                <div style={{marginBottom: '35px'}}>
                {
                    React.Children.toArray(
                        this.state.messages.map(item => {
                            return (
                                <div style={{ marginBottom: '15px', borderBottom: '1px solid black', paddingLeft: '35px' }}>
                                    <h3>{item.userName}</h3>
                                    <p style={{ paddingLeft: '15px' }}>{item.message}</p>
                                </div>
                            )
                        })
                    )
                }
                </div>
                <div style={{ display: "flex", position: 'fixed', bottom: 0, width: '100%', height: '40px' }}>
                    <Input value={this.state.currentMessage} onPressEnter={this.handleSendMessage} onChange={event => { this.setState({ currentMessage: event.target.value }) }} />
                    <Button onClick={this.handleSendMessage} style={{ paddingLeft: '50px', height: '40px', paddingRight: '50px', background: '#686058' }} type="primary">Send</Button>
                </div>
            </LayoutPage>
        );
    };
}
export default ChatPage;