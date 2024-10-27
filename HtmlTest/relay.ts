import { Relay, useWebSocketImplementation } from 'nostr-tools/relay'
useWebSocketImplementation(WebSocket)

const relayUrls = [
    'wss://030939.xyz',
    'wss://relay.nostr.net',
    'wss://nostr-relay.app',
    'wss://purplerelay.com',
    'wss://soloco.nl',
    'wss://nostr.blockpower.capital',
    'wss://nostr.hexhex.online',
    'wss://relay.lumina.rocks',
];

const delay = (ms: number) => new Promise(res => setTimeout(res, ms));
function getRandomElements(arr: string[], numElements: number) {
    // Shuffle the array
    const shuffled = arr.sort(() => 0.5 - Math.random());
    // Get the first numElements elements from the shuffled array
    return shuffled.slice(0, numElements);
}

async function testRelay(url: string): Promise<string> {
    try {
        const relay = await Relay.connect(url);
        console.log(`connected to ${relay.url}`);
        relay.close();
        console.log(`disconnected from ${relay.url}`);
        return relay.url;
    } catch (error) {
        console.log(`${url}: ${error}`);
        await delay(1000);
        return '';
    }
}

async function testRelays(): Promise<string> {
    const promises = getRandomElements(relayUrls, 3).map(r => testRelay(r));
    return await Promise.race(promises);
}

export function exe() {
    testRelays().then((res) => { console.log(`fastest relay is: ${res}`); }, (error) => { console.log(error); });
}
