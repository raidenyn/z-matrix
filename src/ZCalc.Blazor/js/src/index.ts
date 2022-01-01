import * as THREE from "three"
import {CSS2DObject, CSS2DRenderer} from "three/examples/jsm/renderers/CSS2DRenderer"
import {TrackballControls} from "three/examples/jsm/controls/TrackballControls"
import {Molecule} from "./molecule"
import {getGeometry} from "./geometry"
import {PerspectiveCamera, Group, WebGLRenderer, Scene} from "three";

let camera: PerspectiveCamera, scene: Scene, renderer: WebGLRenderer, labelRenderer: CSS2DRenderer
let controls: TrackballControls

let root: Group

const offset = new THREE.Vector3()

export function init(params: { elementId: string }) {

    const container = document.getElementById( params.elementId ) ?? new HTMLElement()
    
    scene = new THREE.Scene()
    scene.background = new THREE.Color( 0x7b79c4 )

    camera = new THREE.PerspectiveCamera( 70, container.offsetWidth / container.offsetHeight, 1, 5000 )
    camera.position.z = 1000
    scene.add( camera )

    const light1 = new THREE.DirectionalLight( 0xffffff, 0.8 )
    light1.position.set( 1, 1, 1 )
    scene.add( light1 )

    const light2 = new THREE.DirectionalLight( 0xffffff, 0.5 )
    light2.position.set( - 1, - 1, 1 )
    scene.add( light2 )

    root = new THREE.Group()
    scene.add( root )

    //
    
    renderer = new THREE.WebGLRenderer( { antialias: true } )
    renderer.setPixelRatio( window.devicePixelRatio )
    renderer.setSize( container.offsetWidth, container.offsetHeight )
    container.appendChild( renderer.domElement )
        
    
    labelRenderer = new CSS2DRenderer()
    labelRenderer.setSize( container.offsetWidth, container.offsetHeight )
    labelRenderer.domElement.style.position = 'absolute'
    labelRenderer.domElement.style.top = `${container.offsetTop}px`;
    labelRenderer.domElement.style.pointerEvents = 'none'
    container.appendChild( labelRenderer.domElement )
    //
 
    controls = new TrackballControls( camera, renderer.domElement )
    controls.minDistance = 500
    controls.maxDistance = 2000

    //
    new ResizeObserver(onContainerResize).observe(container)

    animate()
}

export function setMolecule(molecule: Molecule) {
    
    // remove all existing atoms
    while ( root.children.length > 0 ) {

        const object = root.children[ 0 ]
        object?.parent?.remove( object )

    }

    const geometry = getGeometry(molecule)

    const geometryAtoms = geometry.geometryAtoms
    const geometryBonds = geometry.geometryBonds

    const boxGeometry = new THREE.BoxGeometry( 1, 1, 1 )
    const sphereGeometry = new THREE.IcosahedronGeometry( 1, 3 )

    geometryAtoms.computeBoundingBox()
    geometryAtoms.boundingBox?.getCenter( offset ).negate()

    geometryAtoms.translate( offset.x, offset.y, offset.z )
    geometryBonds.translate( offset.x, offset.y, offset.z )

    let positions = geometryAtoms.getAttribute( 'position' )
 
    const position = new THREE.Vector3()
    const color = new THREE.Color()

    for ( let i = 0; i < positions.count; i ++ )
    {
        const atom = molecule.atoms[ i ]
        
        position.x = positions.getX( i )
        position.y = positions.getY( i )
        position.z = positions.getZ( i )

        color.r = atom.color.r / 255
        color.g = atom.color.g / 255
        color.b = atom.color.b / 255

        const material = new THREE.MeshPhongMaterial( { color: color } )

        const object = new THREE.Mesh( sphereGeometry, material )
        object.position.copy( position )
        object.position.multiplyScalar( 75 )
        object.scale.multiplyScalar( 25 )
        root.add( object )

        const text = document.createElement( 'div' )
        text.className = 'label'
        text.style.color = `rgb(${atom.color.r},${atom.color.g},${atom.color.b})`
        text.textContent = atom.element

        const label = new CSS2DObject( text )
        label.position.copy( object.position )
        root.add( label )

    }

    positions = geometryBonds.getAttribute( 'position' )

    const start = new THREE.Vector3()
    const end = new THREE.Vector3()

    for ( let i = 0; i < positions.count; i += 2 ) {

        start.x = positions.getX( i )
        start.y = positions.getY( i )
        start.z = positions.getZ( i )

        end.x = positions.getX( i + 1 )
        end.y = positions.getY( i + 1 )
        end.z = positions.getZ( i + 1 )

        start.multiplyScalar( 75 )
        end.multiplyScalar( 75 )

        const object = new THREE.Mesh( boxGeometry, new THREE.MeshPhongMaterial( { color: 0xffffff } ) )
        object.position.copy( start )
        object.position.lerp( end, 0.5 )
        object.scale.set( 5, 5, start.distanceTo( end ) )
        object.lookAt( end )
        root.add( object )

    }

    render()

}

//
let resize = true;
function onContainerResize(entries: ResizeObserverEntry[])
{
    if (resize) {
        const container = entries[0].target as HTMLElement
        camera.aspect = container.offsetWidth / container.offsetHeight
        camera.updateProjectionMatrix()

        renderer.setSize( container.offsetWidth, container.offsetHeight )
        labelRenderer.setSize( container.offsetWidth, container.offsetHeight )

        render()
    }
    resize = !resize
}

function animate() {

    requestAnimationFrame( animate )
    controls.update()

    const time = Date.now() * 0.0004

    root.rotation.x = time
    root.rotation.y = time * 0.7

    render()

}

function render() {

    renderer.render( scene, camera )
    labelRenderer.render( scene, camera )

}
