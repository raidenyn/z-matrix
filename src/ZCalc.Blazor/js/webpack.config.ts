import * as path from 'path'

const rootDir = path.resolve(__dirname)
const srcDir = path.resolve(rootDir, 'src')
const outputDir = path.resolve(rootDir, '../wwwroot/js')

export default (() => ({
    devtool: 'source-map',
    entry: {
        'zcalc': path.resolve(srcDir, 'index.ts'),
    },
    target: ['web', 'es2020'],
    module: {
        rules: [{
            exclude: [/node_modules/],
            test: /\.tsx?$/,
            use: [{
                loader: 'ts-loader',
                options: {
                    configFile: path.resolve(srcDir, 'tsconfig.json'),
                    onlyCompileBundledFiles: true,
                },
            }],
        }],
    },
    output: {
        filename: '[name].js',
        path: outputDir,
        libraryTarget: 'umd',
        library: "zcalc",
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js'],
    },
}))
