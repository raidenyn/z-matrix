import * as THREE from "three";
import {Molecule} from "./molecule";

export function getGeometry(molecule: Molecule) {
    const build = {
        geometryAtoms: new THREE.BufferGeometry(),
        geometryBonds: new THREE.BufferGeometry(),
        molecule: molecule
    };
    const geometryAtoms = build.geometryAtoms
    const geometryBonds = build.geometryBonds
    const verticesAtoms = new Array<number>()
    const colorsAtoms = new Array<number>()
    const verticesBonds = new Array<number>()

    for (const atom of molecule.atoms) {
        const { x, y, z } = atom.point
        verticesAtoms.push( x, y, z );
        const { r, g, b } = atom.color
        colorsAtoms.push( r, g, b );
    }


    for (const bond of molecule.bonds) {
        const startAtom = molecule.atoms[ bond.atom1 ];
        const endAtom = molecule.atoms[ bond.atom2 ];
        const { x: x1, y: y1, z: z1 } = startAtom.point
        verticesBonds.push( x1, y1, z1 );
        const { x: x2, y: y2, z: z2 } = endAtom.point
        verticesBonds.push( x2, y2, z2 );

    } // build geometry


    geometryAtoms.setAttribute( 'position', new THREE.Float32BufferAttribute( verticesAtoms, 3 ) );
    geometryAtoms.setAttribute( 'color', new THREE.Float32BufferAttribute( colorsAtoms, 3 ) );
    geometryBonds.setAttribute( 'position', new THREE.Float32BufferAttribute( verticesBonds, 3 ) );
    return build;
}
