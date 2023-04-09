#!/usr/bin/env python

"""update.py: Script to update all appsettings.json before deployment"""

__author__ = "Quan Nguyen"
__copyright__ = "Copyright 2023, Planet Earth"

# Setup parameters
MAIN_PACKAGE = 'Qna.Game.OnlineServer'
API_PROJECT_NAME = MAIN_PACKAGE + '.HttpApi.Host'
SIGNALR_PROJECT_NAME = MAIN_PACKAGE + '.SignalR.Host'
MIGRATOR_PROJECT_NAME = MAIN_PACKAGE + '.DbMigrator'

API_SWAGGER_CLIENT_ID = 'OnlineServer_Swagger'

# import modules used here -- sys is a very standard one
import argparse
import json
import logging
import os


def get_env_data_as_dict(path: str) -> dict:
    with open(path, 'r') as f:
        lines = f.readlines()
        d = {}
        for line in lines:
            if line.startswith('#') or len(line) == 0:
                continue
            line = line.replace('\r', '').replace('\n', '').strip()
            if len(line) == 0:
                continue
            parts = line.split('=')
            d[parts[0]] = line[len(parts[0]) + 1:]

        d['DB_CONNECTION_STRING'] = 'Host={0};Port={1};Database={2};User ID={3};Password={4};'.format(
            d['DB_HOST'],
            d['DB_PORT'],
            d['DB_NAME'],
            d['DB_USERNAME'],
            d['DB_PASSWORD']
        )
    return d


def write_api_settings(env: dict, path: str, outpath: str):
    # Load the JSON file
    with open(path, "r") as file:
        data = json.load(file)

    # Replace the value of the child property
    data["App"]["SelfUrl"] = env['API_SELF_URL']
    data['App']['ClientUrl'] = env['ANGULAR_SELF_URL']
    data['App']['CorsOrigins'] = ','.join([env['ANGULAR_SELF_URL'], env['SIGNALR_SELF_URL']])
    data['App']['RedirectAllowedUrls'] = env['ANGULAR_SELF_URL']

    data['AuthServer']['Authority'] = env['API_SELF_URL']
    data['AuthServer']['SwaggerClientId'] = env['API_SWAGGER_CLIENT_ID']

    data['ConnectionStrings']['Default'] = env['DB_CONNECTION_STRING']

    # Save the updated JSON back to the file
    out_dir = os.path.dirname(outpath)
    if not os.path.exists(out_dir):
        os.makedirs(out_dir)
    with open(outpath, "w") as file:
        json.dump(data, file, indent=4)
    logging.info('done create settings for api')


def write_signalr_settings(env: dict, path: str, outpath: str):
    # Load the JSON file
    with open(path, "r") as file:
        data = json.load(file)

    # Replace the value of the child property
    data["App"]["SelfUrl"] = env['SIGNALR_SELF_URL']
    data['App']['CorsOrigins'] = ','.join([env['SIGNALR_SELF_URL']])
    data['App']['RedirectAllowedUrls'] = env['ANGULAR_SELF_URL']

    data['AuthServer']['Authority'] = env['API_SELF_URL']
    data['AuthServer']['SwaggerClientId'] = env['SIGNALR_SWAGGER_CLIENT_ID']

    data['ConnectionStrings']['Default'] = env['DB_CONNECTION_STRING']

    # Save the updated JSON back to the file
    out_dir = os.path.dirname(outpath)
    if not os.path.exists(out_dir):
        os.makedirs(out_dir)
    with open(outpath, "w") as file:
        json.dump(data, file, indent=4)
    logging.info('done create settings for signalr')


def write_migrator_settings(env: dict, path: str, outpath: str):
    # Load the JSON file
    with open(path, "r") as file:
        data = json.load(file)

    # Replace the value of the child property
    data['ConnectionStrings']['Default'] = env['DB_CONNECTION_STRING']

    # Save the updated JSON back to the file
    out_dir = os.path.dirname(outpath)
    if not os.path.exists(out_dir):
        os.makedirs(out_dir)
    with open(outpath, "w") as file:
        json.dump(data, file, indent=4)
    logging.info('done create settings for migrator')


# Gather our code in a main() function
def main(args, loglevel):
    logging.basicConfig(format="%(levelname)s: %(message)s", level=loglevel)
    logging.info("Your Argument: %s" % args)
    env_name = args.env
    logging.info("Your Env: %s" % env_name)

    script_dir = os.path.dirname(os.path.realpath(__file__))
    parent_dir = os.path.dirname(script_dir)
    src_dir = os.path.join(parent_dir, 'src')
    env_file = os.path.join(script_dir, env_name + '.env')
    env_params = get_env_data_as_dict(env_file)
    logging.debug(env_params)

    write_api_settings(env_params,
                       os.path.join(src_dir, API_PROJECT_NAME, 'appsettings.json'),
                       os.path.join(script_dir, 'out', 'Api', 'appsettings.json'))
    write_signalr_settings(env_params,
                           os.path.join(src_dir, SIGNALR_PROJECT_NAME, 'appsettings.json'),
                           os.path.join(script_dir, 'out', 'SignalR', 'appsettings.json'))
    write_migrator_settings(env_params,
                            os.path.join(src_dir, MIGRATOR_PROJECT_NAME, 'appsettings.json'),
                            os.path.join(script_dir, 'out', 'Migrator', 'appsettings.json'))


# Standard boilerplate to call the main() function to begin the program.
if __name__ == '__main__':
    parser = argparse.ArgumentParser(
        description="Does a thing to some stuff.",
        epilog="As an alternative to the commandline, params can be placed in a file, one per line, and specified on the commandline like '%(prog)s @params.conf'.",
        fromfile_prefix_chars='@')
    parser.add_argument(
        "env",
        help="pass env to the program",
        metavar="env")
    parser.add_argument(
        "-v",
        "--verbose",
        help="increase output verbosity",
        action="store_true")
    args = parser.parse_args()

    # Setup logging
    if args.verbose:
        loglevel = logging.DEBUG
    else:
        loglevel = logging.INFO

    main(args, loglevel)
